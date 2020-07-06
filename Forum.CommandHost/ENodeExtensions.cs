using System.Collections.Generic;
using System.Net;
using System.Reflection;
using ENode.Configurations;
using ENode.EQueue;
using ENode.Eventing;
using ENode.Messaging;
using EQueue.Configurations;
using EQueue.Clients.Consumers;
using EQueue.Clients.Producers;
using Forum.Infrastructure;
using EQueue.Protocols;
using ENode.Commanding;

namespace Forum.CommandHost
{
    public static class ENodeExtensions
    {
        private static CommandService _commandService;
        private static CommandConsumer _commandConsumer;
        private static DomainEventPublisher _eventPublisher;
        private static DomainEventConsumer _eventConsumer;

        public static ENodeConfiguration BuildContainer(this ENodeConfiguration enodeConfiguration)
        {
            enodeConfiguration.GetCommonConfiguration().BuildContainer();
            return enodeConfiguration;
        }
        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly() };
            enodeConfiguration.RegisterTopicProviders(assemblies);

            var configuration = enodeConfiguration.GetCommonConfiguration();
            configuration.RegisterEQueueComponents();

            _commandService = new CommandService();
            configuration.SetDefault<ICommandService, CommandService>(_commandService);

            _eventPublisher = new DomainEventPublisher();
            configuration.SetDefault<IMessagePublisher<DomainEventStreamMessage>, DomainEventPublisher>(_eventPublisher);

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var nameServerEndpoint = new IPEndPoint(IPAddress.Loopback, ConfigSettings.NameServerPort);
            var nameServerEndpoints = new List<IPEndPoint> { nameServerEndpoint };

            _commandService.InitializeEQueue(null, new ProducerSetting
            {
                NameServerList = nameServerEndpoints
            });

            _eventPublisher.InitializeEQueue(new ProducerSetting
            {
                NameServerList = nameServerEndpoints
            });

            _commandConsumer = new CommandConsumer().InitializeEQueue(setting: new ConsumerSetting
            {
                NameServerList = nameServerEndpoints,
                ConsumeFromWhere = ConsumeFromWhere.FirstOffset
            });
            _commandConsumer
                .Subscribe("AccountCommandTopic")
                .Subscribe("SectionCommandTopic")
                .Subscribe("PostCommandTopic")
                .Subscribe("ReplyCommandTopic");

            _eventConsumer = new DomainEventConsumer().InitializeEQueue("CommandHostGroup", setting: new ConsumerSetting
            {
                NameServerList = nameServerEndpoints,
                ConsumeFromWhere = ConsumeFromWhere.FirstOffset
            });
            _eventConsumer.Subscribe("ReplyEventTopic");

            _commandService.Start();
            _eventConsumer.Start();
            _commandConsumer.Start();
            _eventPublisher.Start();

            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _eventConsumer.Shutdown();
            _commandConsumer.Shutdown();
            _eventPublisher.Shutdown();
            _commandService.Shutdown();
            return enodeConfiguration;
        }
    }
}
