using System.Collections.Generic;
using System.Net;
using System.Reflection;
using ENode.Configurations;
using ENode.EQueue;
using ENode.Eventing;
using ENode.Infrastructure;
using EQueue.Clients.Consumers;
using EQueue.Clients.Producers;
using EQueue.Configurations;
using Forum.Infrastructure;

namespace Forum.CommandService
{
    public static class ENodeExtensions
    {
        private static CommandConsumer _commandConsumer;
        private static DomainEventPublisher _eventPublisher;

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

            _eventPublisher = new DomainEventPublisher();
            configuration.SetDefault<IMessagePublisher<DomainEventStreamMessage>, DomainEventPublisher>(_eventPublisher);

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var nameServerEndpoint = new IPEndPoint(IPAddress.Loopback, ConfigSettings.NameServerPort);
            var nameServerEndpoints = new List<IPEndPoint> { nameServerEndpoint };

            _eventPublisher.Initialize(new ProducerSetting
            {
                NameServerList = nameServerEndpoints
            });

            _commandConsumer = new CommandConsumer().Initialize(setting: new ConsumerSetting
            {
                NameServerList = nameServerEndpoints
            });
            _commandConsumer
                .Subscribe("AccountCommandTopic")
                .Subscribe("SectionCommandTopic")
                .Subscribe("PostCommandTopic")
                .Subscribe("ReplyCommandTopic");

            _commandConsumer.Start();
            _eventPublisher.Start();

            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandConsumer.Shutdown();
            _eventPublisher.Shutdown();
            return enodeConfiguration;
        }
    }
}
