using System.Collections.Generic;
using System.Net;
using System.Reflection;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using EQueue.Clients.Producers;
using EQueue.Configurations;
using Forum.Infrastructure;

namespace Forum.Web
{
    public static class ENodeExtensions
    {
        private static CommandService _commandService;

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
            var commandResultProcessor = new CommandResultProcessor().Initialize(new IPEndPoint(IPAddress.Loopback, 9000));

            _commandService.Initialize(commandResultProcessor, new ProducerSetting
            {
                NameServerList = nameServerEndpoints
            });

            _commandService.Start();

            return enodeConfiguration;
        }
    }
}