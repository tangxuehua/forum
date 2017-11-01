using System.Net;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using EQueue.Configurations;
using EQueue.NameServer;
using Forum.Infrastructure;
using ECommonConfiguration = ECommon.Configurations.Configuration;

namespace Forum.NameServerService
{
    public class Bootstrap
    {
        private static ECommonConfiguration _ecommonConfiguration;
        private static NameServerController _nameServer;

        public static void Initialize()
        {
            InitializeEQueue();
        }
        public static void Start()
        {
            _nameServer.Start();
        }
        public static void Stop()
        {
            if (_nameServer != null)
            {
                _nameServer.Shutdown();
            }
        }

        private static void InitializeEQueue()
        {
            _ecommonConfiguration = ECommonConfiguration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .RegisterEQueueComponents()
                .BuildContainer();
            ConfigSettings.Initialize();
            var setting = new NameServerSetting()
            {
                BindingAddress = new IPEndPoint(IPAddress.Loopback, ConfigSettings.NameServerPort)
            };
            _nameServer = new NameServerController(setting);
            ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Bootstrap).FullName).Info("NameServer initialized.");
        }
    }
}
