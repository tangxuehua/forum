using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using ECommon.Components;
using ECommon.Extensions;
using ECommon.Logging;
using ECommon.Scheduling;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using ENode.Eventing;
using ENode.Infrastructure;
using EQueue.Broker;
using EQueue.Clients.Consumers;
using EQueue.Clients.Producers;
using EQueue.Configurations;
using EQueue.NameServer;
using Forum.Infrastructure;

namespace Forum.Tests
{
    public static class ENodeExtensions
    {
        private static NameServerController _nameServerController;
        private static BrokerController _broker;
        private static CommandService _commandService;
        private static CommandConsumer _commandConsumer;
        private static DomainEventPublisher _eventPublisher;
        private static DomainEventConsumer _eventConsumer;
        private static bool _isEQueueInitialized;
        private static bool _isEQueueStarted;

        public static ENodeConfiguration BuildContainer(this ENodeConfiguration enodeConfiguration)
        {
            enodeConfiguration.GetCommonConfiguration().BuildContainer();
            return enodeConfiguration;
        }
        public static ENodeConfiguration InitializeEQueue(this ENodeConfiguration enodeConfiguration)
        {
            if (_isEQueueInitialized)
            {
                return enodeConfiguration;
            }

            _commandService = new CommandService();
            _eventPublisher = new DomainEventPublisher();

            _isEQueueInitialized = true;

            return enodeConfiguration;
        }
        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly() };
            enodeConfiguration.RegisterTopicProviders(assemblies);

            var configuration = enodeConfiguration.GetCommonConfiguration();
            configuration.RegisterEQueueComponents();

            configuration.SetDefault<ICommandService, CommandService>(_commandService);
            configuration.SetDefault<IMessagePublisher<DomainEventStreamMessage>, DomainEventPublisher>(_eventPublisher);

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            if (_isEQueueStarted)
            {
                _commandService.InitializeENode();
                _eventPublisher.InitializeENode();

                _commandConsumer.InitializeENode();
                _eventConsumer.InitializeENode();

                return enodeConfiguration;
            }

            var nameServerEndpoint = new IPEndPoint(IPAddress.Loopback, ConfigSettings.NameServerPort);
            var nameServerEndpoints = new List<IPEndPoint> { nameServerEndpoint };
            var nameServerSetting = new NameServerSetting()
            {
                BindingAddress = nameServerEndpoint
            };
            _nameServerController = new NameServerController(nameServerSetting);

            var brokerStorePath = @"c:\forum-equeue-store-test";
            if (Directory.Exists(brokerStorePath))
            {
                Directory.Delete(brokerStorePath, true);
            }

            var brokerSetting = new BrokerSetting(false, brokerStorePath)
            {
                NameServerList = nameServerEndpoints
            };

            brokerSetting.BrokerInfo.ProducerAddress = new IPEndPoint(IPAddress.Loopback, ConfigSettings.BrokerProducerPort).ToAddress();
            brokerSetting.BrokerInfo.ConsumerAddress = new IPEndPoint(IPAddress.Loopback, ConfigSettings.BrokerConsumerPort).ToAddress();
            brokerSetting.BrokerInfo.AdminAddress = new IPEndPoint(IPAddress.Loopback, ConfigSettings.BrokerAdminPort).ToAddress();
            _broker = BrokerController.Create(brokerSetting);

            _commandService.InitializeEQueue(new CommandResultProcessor().Initialize(new IPEndPoint(IPAddress.Loopback, 9000)), new ProducerSetting
            {
                NameServerList = nameServerEndpoints
            });
            _eventPublisher.InitializeEQueue(new ProducerSetting
            {
                NameServerList = nameServerEndpoints
            });
            _commandConsumer = new CommandConsumer().InitializeEQueue(setting: new ConsumerSetting
            {
                NameServerList = nameServerEndpoints
            });
            _eventConsumer = new DomainEventConsumer().InitializeEQueue(setting: new ConsumerSetting
            {
                NameServerList = nameServerEndpoints
            });

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

            _nameServerController.Start();
            _broker.Start();
            _eventConsumer.Start();
            _commandConsumer.Start();
            _eventPublisher.Start();
            _commandService.Start();

            WaitAllConsumerLoadBalanceComplete();

            _isEQueueStarted = true;

            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandService.Shutdown();
            _eventPublisher.Shutdown();
            _commandConsumer.Shutdown();
            _eventConsumer.Shutdown();
            _broker.Shutdown();
            _nameServerController.Shutdown();
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