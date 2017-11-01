using System.Collections.Generic;
using System.Net;
using System.Reflection;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using EQueue.Clients.Consumers;
using EQueue.Clients.Producers;
using EQueue.Configurations;
using Forum.Infrastructure;

namespace Forum.EventService
{
    public static class ENodeExtensions
    {
        private static CommandService _commandService;
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

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var nameServerEndpoint = new IPEndPoint(IPAddress.Loopback, ConfigSettings.NameServerPort);
            var nameServerEndpoints = new List<IPEndPoint> { nameServerEndpoint };

            _commandService.Initialize(setting: new ProducerSetting
            {
                NameServerList = nameServerEndpoints
            });

            _eventConsumer = new DomainEventConsumer().Initialize(setting: new ConsumerSetting
            {
                NameServerList = nameServerEndpoints
            });
            _eventConsumer
                .Subscribe("AccountEventTopic")
                .Subscribe("SectionEventTopic")
                .Subscribe("PostEventTopic")
                .Subscribe("ReplyEventTopic");

            _commandService.Start();
            _eventConsumer.Start();

            return enodeConfiguration;
        }
        public static ENodeConfiguration ShutdownEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _eventConsumer.Shutdown();
            _commandService.Shutdown();
            return enodeConfiguration;
        }
    }
}
