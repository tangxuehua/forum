using System.Linq;
using System.Threading;
using ECommon.Components;
using ECommon.Extensions;
using ECommon.Logging;
using ECommon.Scheduling;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using ENode.EQueue.Commanding;
using ENode.Eventing;
using ENode.Infrastructure;
using ENode.Messaging;
using EQueue.Broker;
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
        private static MessagePublisher _messagePublisher;
        private static MessageConsumer _messageConsumer;
        private static CommandResultProcessor _commandResultProcessor;

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _broker = new BrokerController();
            _commandResultProcessor = new CommandResultProcessor();
            _commandService = new CommandService(_commandResultProcessor);
            _eventPublisher = new EventPublisher();
            _messagePublisher = new MessagePublisher();

            configuration.SetDefault<ICommandService, CommandService>(_commandService);
            configuration.SetDefault<IPublisher<EventStream>, EventPublisher>(_eventPublisher);
            configuration.SetDefault<IPublisher<DomainEventStream>, EventPublisher>(_eventPublisher);
            configuration.SetDefault<IPublisher<IMessage>, MessagePublisher>(_messagePublisher);

            _commandConsumer = new CommandConsumer();
            _eventConsumer = new EventConsumer();
            _messageConsumer = new MessageConsumer();

            ObjectContainer.Resolve<ITopicProvider<ICommand>>().GetAllTopics().ForEach(topic => _commandConsumer.Subscribe(topic));
            ObjectContainer.Resolve<ITopicProvider<IEvent>>().GetAllTopics().ForEach(topic => _eventConsumer.Subscribe(topic));
            ObjectContainer.Resolve<ITopicProvider<IMessage>>().GetAllTopics().ForEach(topic => _messageConsumer.Subscribe(topic));

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _broker.Start();
            _eventConsumer.Start();
            _messageConsumer.Start();
            _commandConsumer.Start();
            _eventPublisher.Start();
            _messagePublisher.Start();
            _commandService.Start();
            _commandResultProcessor.Start();

            WaitAllConsumerLoadBalanceComplete();

            return enodeConfiguration;
        }

        private static void WaitAllConsumerLoadBalanceComplete()
        {
            var scheduleService = ObjectContainer.Resolve<IScheduleService>();
            var waitHandle = new ManualResetEvent(false);
            var totalCommandTopicCount = ObjectContainer.Resolve<ITopicProvider<ICommand>>().GetAllTopics().Count();
            var totalEventTopicCount = ObjectContainer.Resolve<ITopicProvider<IEvent>>().GetAllTopics().Count();
            var totalMessageTopicCount = ObjectContainer.Resolve<ITopicProvider<IMessage>>().GetAllTopics().Count();

            var logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(ENodeExtensions).Name);
            logger.Info("Waiting for all consumer load balance complete, please wait for a moment...");
            var taskId = scheduleService.ScheduleTask("WaitAllConsumerLoadBalanceComplete", () =>
            {
                var messageConsumerAllocatedQueues = _messageConsumer.Consumer.GetCurrentQueues();
                var eventConsumerAllocatedQueues = _eventConsumer.Consumer.GetCurrentQueues();
                var commandConsumerAllocatedQueues = _commandConsumer.Consumer.GetCurrentQueues();
                var executedCommandMessageConsumerAllocatedQueues = _commandResultProcessor.CommandExecutedMessageConsumer.GetCurrentQueues();
                var domainEventHandledMessageConsumerAllocatedQueues = _commandResultProcessor.DomainEventHandledMessageConsumer.GetCurrentQueues();
                if (messageConsumerAllocatedQueues.Count() == totalMessageTopicCount * _broker.Setting.DefaultTopicQueueCount
                    && eventConsumerAllocatedQueues.Count() == totalCommandTopicCount * _broker.Setting.DefaultTopicQueueCount
                    && commandConsumerAllocatedQueues.Count() == totalEventTopicCount * _broker.Setting.DefaultTopicQueueCount
                    && executedCommandMessageConsumerAllocatedQueues.Count() == 4
                    && domainEventHandledMessageConsumerAllocatedQueues.Count() == 4)
                {
                    waitHandle.Set();
                }
            }, 1000, 1000);

            waitHandle.WaitOne();
            scheduleService.ShutdownTask(taskId);
            logger.Info("All consumer load balance completed.");
        }
    }
}