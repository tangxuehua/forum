using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using ECommon.Components;
using ECommon.Logging;
using ECommon.Scheduling;
using ECommon.Socketing;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using ENode.EQueue.Commanding;
using ENode.Eventing;
using ENode.Infrastructure;
using EQueue.Broker;
using EQueue.Configurations;

namespace Forum.Domain.Tests
{
    public static class ENodeExtensions
    {
        private static BrokerController _broker;
        private static CommandService _commandService;
        private static CommandConsumer _commandConsumer;
        private static DomainEventPublisher _eventPublisher;
        private static DomainEventConsumer _eventConsumer;
        private static CommandResultProcessor _commandResultProcessor;

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();
            var brokerStorePath = @"d:\equeue-store-test";

            if (Directory.Exists(brokerStorePath))
            {
                Directory.Delete(brokerStorePath, true);
            }

            configuration.RegisterEQueueComponents();

            _broker = BrokerController.Create(new BrokerSetting(brokerStorePath));
            _commandResultProcessor = new CommandResultProcessor(new IPEndPoint(SocketUtils.GetLocalIPV4(), 9000));
            _commandService = new CommandService(_commandResultProcessor);
            _eventPublisher = new DomainEventPublisher();

            configuration.SetDefault<ICommandService, CommandService>(_commandService);
            configuration.SetDefault<IMessagePublisher<DomainEventStreamMessage>, DomainEventPublisher>(_eventPublisher);

            _commandConsumer = new CommandConsumer();
            _eventConsumer = new DomainEventConsumer();

            _commandConsumer
                .Subscribe("AccountCommandTopic")
                .Subscribe("SectionCommandTopic")
                .Subscribe("PostCommandTopic")
                .Subscribe("ReplyCommandTopic");
            _eventConsumer
                .Subscribe("AccountEventTopic")
                .Subscribe("SectionEventTopic")
                .Subscribe("PostEventTopic")
                .Subscribe("ReplyEventTopic");

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _broker.Start();
            _eventConsumer.Start();
            _commandConsumer.Start();
            _eventPublisher.Start();
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
            var totalEventTopicCount = ObjectContainer.Resolve<ITopicProvider<IDomainEvent>>().GetAllTopics().Count();

            var logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(ENodeExtensions).Name);
            logger.Info("Waiting for all consumer load balance complete, please wait for a moment...");
            scheduleService.StartTask("WaitAllConsumerLoadBalanceComplete", () =>
            {
                var eventConsumerAllocatedQueues = _eventConsumer.Consumer.GetCurrentQueues();
                var commandConsumerAllocatedQueues = _commandConsumer.Consumer.GetCurrentQueues();
                if (eventConsumerAllocatedQueues.Count() == totalEventTopicCount * _broker.Setting.TopicDefaultQueueCount
                    && commandConsumerAllocatedQueues.Count() == totalCommandTopicCount * _broker.Setting.TopicDefaultQueueCount)
                {
                    waitHandle.Set();
                }
            }, 1000, 1000);

            waitHandle.WaitOne();
            scheduleService.StopTask("WaitAllConsumerLoadBalanceComplete");
            logger.Info("All consumer load balance completed.");
        }
    }
}