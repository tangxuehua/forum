using System.Linq;
using System.Threading;
using ECommon.Components;
using ECommon.Remoting;
using ECommon.Scheduling;
using ECommon.Socketing;
using ENode.Commanding;
using ENode.Configurations;
using ENode.Domain;
using ENode.EQueue;
using ENode.EQueue.Commanding;
using ENode.Eventing;
using EQueue.Broker;
using EQueue.Clients.Consumers;
using EQueue.Clients.Producers;
using EQueue.Configurations;

namespace Forum.Domain.Tests
{
    public static class ENodeExtensions
    {
        private static BrokerController _broker;
        private static CommandService _commandService;
        private static CommandConsumer _commandConsumer;
        private static EventPublisher _eventPublisher;
        private static EventConsumer _eventConsumer;
        private static CommandExecutedMessageSender _commandExecutedMessageSender;
        private static DomainEventHandledMessageSender _domainEventHandledMessageSender;
        private static CommandResultProcessor _commandResultProcessor;

        public static ENodeConfiguration SetProviders(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();
            configuration.SetDefault<ICommandTopicProvider, CommandTopicProvider>();
            configuration.SetDefault<ICommandTypeCodeProvider, CommandTypeCodeProvider>();
            configuration.SetDefault<IAggregateRootTypeCodeProvider, AggregateRootTypeCodeProvider>();
            configuration.SetDefault<IEventTopicProvider, EventTopicProvider>();
            configuration.SetDefault<IEventTypeCodeProvider, EventTypeCodeProvider>();
            configuration.SetDefault<IEventHandlerTypeCodeProvider, EventHandlerTypeCodeProvider>();
            return enodeConfiguration;
        }
        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            var producerSetting = new ProducerSetting { BrokerPort = 6000 };
            var consumerSetting = new ConsumerSetting
            {
                BrokerPort = 6001,
                HeartbeatBrokerInterval = 1000,
                UpdateTopicQueueCountInterval = 1000,
                RebalanceInterval = 1000
            };
            var eventConsumerSetting = new ConsumerSetting
            {
                BrokerPort = 6001,
                HeartbeatBrokerInterval = 1000,
                UpdateTopicQueueCountInterval = 1000,
                RebalanceInterval = 1000,
                MessageHandleMode = MessageHandleMode.Sequential
            };

            var brokerSetting = new BrokerSetting
            {
                ProducerSocketSetting = new SocketSetting { Address = SocketUtils.GetLocalIPV4().ToString(), Port = 6000, Backlog = 5000 },
                ConsumerSocketSetting = new SocketSetting { Address = SocketUtils.GetLocalIPV4().ToString(), Port = 6001, Backlog = 5000 }
            };
            _broker = new BrokerController(brokerSetting).Initialize();

            var commandExecutedMessageConsumer = new Consumer("CommandExecutedMessageConsumer", "CommandExecutedMessageConsumerGroup", consumerSetting);
            var domainEventHandledMessageConsumer = new Consumer("DomainEventHandledMessageConsumer", "DomainEventHandledMessageConsumerGroup", consumerSetting);
            _commandResultProcessor = new CommandResultProcessor(commandExecutedMessageConsumer, domainEventHandledMessageConsumer);

            _commandService = new CommandService(_commandResultProcessor, "CommandService", producerSetting);
            _commandExecutedMessageSender = new CommandExecutedMessageSender("CommandExecutedMessageSender", producerSetting);
            _domainEventHandledMessageSender = new DomainEventHandledMessageSender("DomainEventHandledMessageSender", producerSetting);
            _eventPublisher = new EventPublisher("EventPublisher", producerSetting);

            configuration.SetDefault<ICommandService, CommandService>(_commandService);
            configuration.SetDefault<IEventPublisher, EventPublisher>(_eventPublisher);

            _commandConsumer = new CommandConsumer(consumerSetting, _commandExecutedMessageSender);
            _eventConsumer = new EventConsumer(eventConsumerSetting, _domainEventHandledMessageSender);

            var commandTopicProvider = ObjectContainer.Resolve<ICommandTopicProvider>() as CommandTopicProvider;
            var eventTopicProvider = ObjectContainer.Resolve<IEventTopicProvider>() as EventTopicProvider;

            commandTopicProvider.GetAllCommandTopics().ToList().ForEach(topic => _commandConsumer.Subscribe(topic));
            eventTopicProvider.GetAllEventTopics().ToList().ForEach(topic => _eventConsumer.Subscribe(topic));

            _commandResultProcessor.SetExecutedCommandMessageTopic("ExecutedCommandMessageTopic");
            _commandResultProcessor.SetDomainEventHandledMessageTopic("DomainEventHandledMessageTopic");

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _broker.Start();
            _eventConsumer.Start();
            _commandConsumer.Start();
            _eventPublisher.Start();
            _commandService.Start();
            _commandExecutedMessageSender.Start();
            _domainEventHandledMessageSender.Start();
            _commandResultProcessor.Start();

            WaitAllConsumerLoadBalanceComplete();

            return enodeConfiguration;
        }

        private static void WaitAllConsumerLoadBalanceComplete()
        {
            var scheduleService = ObjectContainer.Resolve<IScheduleService>();
            var waitHandle = new ManualResetEvent(false);
            var commandTopicProvider = ObjectContainer.Resolve<ICommandTopicProvider>() as CommandTopicProvider;
            var eventTopicProvider = ObjectContainer.Resolve<IEventTopicProvider>() as EventTopicProvider;

            var totalCommandTopicCount = commandTopicProvider.GetAllCommandTopics().Count();
            var totalEventTopicCount = eventTopicProvider.GetAllEventTopics().Count();

            var taskId = scheduleService.ScheduleTask(() =>
            {
                var eventConsumerAllocatedQueues = _eventConsumer.Consumer.GetCurrentQueues();
                var commandConsumerAllocatedQueues = _commandConsumer.Consumer.GetCurrentQueues();
                var executedCommandMessageConsumerAllocatedQueues = _commandResultProcessor.CommandExecutedMessageConsumer.GetCurrentQueues();
                var domainEventHandledMessageConsumerAllocatedQueues = _commandResultProcessor.DomainEventHandledMessageConsumer.GetCurrentQueues();
                if (eventConsumerAllocatedQueues.Count() == totalCommandTopicCount * _broker.Setting.DefaultTopicQueueCount
                    && commandConsumerAllocatedQueues.Count() == totalEventTopicCount * _broker.Setting.DefaultTopicQueueCount
                    && executedCommandMessageConsumerAllocatedQueues.Count() == 4
                    && domainEventHandledMessageConsumerAllocatedQueues.Count() == 4)
                {
                    waitHandle.Set();
                }
            }, 1000, 1000);

            waitHandle.WaitOne();
            scheduleService.ShutdownTask(taskId);
        }
    }
}