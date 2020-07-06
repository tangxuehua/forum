using System.Net;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ECommon.Serilog;
using EQueue.Configurations;
using EQueue.NameServer;
using Forum.Infrastructure;
using ECommonConfiguration = ECommon.Configurations.Configuration;

namespace Forum.NameServerHost
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
            var loggerFactory = new SerilogLoggerFactory()
                .AddFileLogger("ECommon", "logs\\ecommon")
                .AddFileLogger("EQueue", "logs\\equeue")
                .AddFileLogger("ENode", "logs\\enode");

            _ecommonConfiguration = ECommonConfiguration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseSerilog(loggerFactory)
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .RegisterEQueueComponents()
                .BuildContainer();

            var setting = new NameServerSetting()
            {
                BindingAddress = new IPEndPoint(IPAddress.Loopback, ConfigSettings.NameServerPort)
            };
            _nameServer = new NameServerController(setting);
            ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Bootstrap).FullName).Info("NameServer initialized.");
        }
    }
}
