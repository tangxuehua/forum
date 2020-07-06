using System.Collections.Generic;
using System.Net;
using System.Reflection;
using ENode.Configurations;
using ENode.EQueue;
using EQueue.Clients.Consumers;
using EQueue.Configurations;
using EQueue.Protocols;
using Forum.Infrastructure;

namespace Forum.EventHost
{
    public static class ENodeExtensions
    {
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

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var nameServerEndpoint = new IPEndPoint(IPAddress.Loopback, ConfigSettings.NameServerPort);
            var nameServerEndpoints = new List<IPEndPoint> { nameServerEndpoint };

            _eventConsumer = new DomainEventConsumer().InitializeEQueue("EventHostGroup", setting: new ConsumerSetting
            {
                NameServerList = nameServerEndpoints,
                ConsumeFromWhere = ConsumeFromWhere.FirstOffset
            });
            _eventConsumer
                .Subscribe("AccountEventTopic")
                .Subscribe("SectionEventTopic")
                .Subscribe("PostEventTopic")
                .Subscribe("ReplyEventTopic");

            _eventConsumer.Start();

            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _eventConsumer.Shutdown();
            return enodeConfiguration;
        }
    }
}
