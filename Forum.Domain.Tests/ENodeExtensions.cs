using System.Linq;
using System.Net;
using System.Threading;
using ECommon.Components;
using ECommon.Logging;
using ECommon.Scheduling;
using ECommon.Utilities;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using ENode.EQueue.Commanding;
using ENode.Eventing;
using ENode.Infrastructure;
using ENode.Infrastructure.Impl;
using EQueue.Broker;
using EQueue.Configurations;
using Forum.Commands.Accounts;
using Forum.Commands.Posts;
using Forum.Commands.Replies;
using Forum.Commands.Sections;
using Forum.Denormalizers.Dapper;
using Forum.Domain.Accounts;
using Forum.Domain.Posts;
using Forum.Domain.Replies;
using Forum.Domain.Sections;
using Forum.ProcessManagers;

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

        public static ENodeConfiguration RegisterAllTypeCodes(this ENodeConfiguration enodeConfiguration)
        {
            var provider = ObjectContainer.Resolve<ITypeCodeProvider>() as DefaultTypeCodeProvider;

            //aggregates
            provider.RegisterType<Account>(10);
            provider.RegisterType<Section>(11);
            provider.RegisterType<Post>(12);
            provider.RegisterType<Reply>(13);

            //commands
            provider.RegisterType<RegisterNewAccountCommand>(11000);

            provider.RegisterType<CreateSectionCommand>(11100);
            provider.RegisterType<ChangeSectionNameCommand>(11101);

            provider.RegisterType<CreatePostCommand>(11200);
            provider.RegisterType<UpdatePostCommand>(11201);
            provider.RegisterType<AcceptNewReplyCommand>(11202);

            provider.RegisterType<CreateReplyCommand>(11300);
            provider.RegisterType<ChangeReplyBodyCommand>(11301);

            //domain events
            provider.RegisterType<NewAccountRegisteredEvent>(21000);

            provider.RegisterType<SectionCreatedEvent>(21100);
            provider.RegisterType<SectionNameChangedEvent>(21101);

            provider.RegisterType<PostCreatedEvent>(21200);
            provider.RegisterType<PostUpdatedEvent>(21201);
            provider.RegisterType<PostReplyStatisticInfoChangedEvent>(21202);

            provider.RegisterType<ReplyCreatedEvent>(21300);
            provider.RegisterType<ReplyBodyChangedEvent>(21301);

            //denormalizers
            provider.RegisterType<AccountDenormalizer>(110);
            provider.RegisterType<SectionDenormalizer>(111);
            provider.RegisterType<PostDenormalizer>(112);
            provider.RegisterType<ReplyDenormalizer>(113);

            //process managers and other event handlers
            provider.RegisterType<CreateReplyProcessManager>(1000);

            return enodeConfiguration;
        }
        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _broker = BrokerController.Create();
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
            var taskId = scheduleService.ScheduleTask("WaitAllConsumerLoadBalanceComplete", () =>
            {
                var eventConsumerAllocatedQueues = _eventConsumer.Consumer.GetCurrentQueues();
                var commandConsumerAllocatedQueues = _commandConsumer.Consumer.GetCurrentQueues();
                if (eventConsumerAllocatedQueues.Count() == totalCommandTopicCount * _broker.Setting.TopicDefaultQueueCount
                    && commandConsumerAllocatedQueues.Count() == totalEventTopicCount * _broker.Setting.TopicDefaultQueueCount)
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