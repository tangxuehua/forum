using System;
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
        private static ILogger _logger;
        private static ECommonConfiguration _ecommonConfiguration;
        private static NameServerController _nameServer;

        public static void Initialize()
        {
            InitializeECommon();
            try
            {
                InitializeEQueue();
            }
            catch (Exception ex)
            {
                _logger.Error("Initialize NameServer failed.", ex);
                throw;
            }
        }
        public static void Start()
        {
            try
            {
                _nameServer.Start();
            }
            catch (Exception ex)
            {
                _logger.Error("NameServer start failed.", ex);
                throw;
            }
        }
        public static void Stop()
        {
            try
            {
                if (_nameServer != null)
                {
                    _nameServer.Shutdown();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("NameServer stop failed.", ex);
                throw;
            }
        }

        private static void InitializeECommon()
        {
            _ecommonConfiguration = ECommonConfiguration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler();
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Bootstrap).FullName);
            _logger.Info("ECommon initialized.");
        }
        private static void InitializeEQueue()
        {
            _ecommonConfiguration.RegisterEQueueComponents();
            ConfigSettings.Initialize();
            var setting = new NameServerSetting()
            {
                BindingAddress = new IPEndPoint(IPAddress.Loopback, ConfigSettings.NameServerPort)
            };
            _nameServer = new NameServerController(setting);
            _logger.Info("NameServer initialized.");
        }
    }
}
