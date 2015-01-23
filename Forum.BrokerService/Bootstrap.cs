using System;
using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ECommon.Logging;
using EQueue.Broker;
using EQueue.Configurations;
using Forum.Infrastructure;

namespace Forum.BrokerService
{
    public class Bootstrap
    {
        private static ILogger _logger;
        private static Configuration _ecommonConfiguration;
        private static BrokerController _broker;

        public static void Initialize()
        {
            InitializeECommon();
            try
            {
                InitializeEQueue();
            }
            catch (Exception ex)
            {
                _logger.Error("Initialize EQueue failed.", ex);
                throw;
            }
        }
        public static void Start()
        {
            try
            {
                _broker.Start();
            }
            catch (Exception ex)
            {
                _logger.Error("Broker start failed.", ex);
                throw;
            }
        }
        public static void Stop()
        {
            try
            {
                if (_broker != null)
                {
                    _broker.Shutdown();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Broker stop failed.", ex);
                throw;
            }
        }

        private static void InitializeECommon()
        {
            _ecommonConfiguration = Configuration
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
            ConfigSettings.Initialize();

            var messageStoreSetting = new SqlServerMessageStoreSetting
            {
                ConnectionString = ConfigSettings.ConnectionString,
                DeleteMessageHourOfDay = -1
            };
            var offsetManagerSetting = new SqlServerOffsetManagerSetting
            {
                ConnectionString = ConfigSettings.ConnectionString
            };

            _ecommonConfiguration
                .RegisterEQueueComponents()
                .UseSqlServerMessageStore(messageStoreSetting)
                .UseSqlServerOffsetManager(offsetManagerSetting);

            _broker = new BrokerController();
            _logger.Info("EQueue initialized.");
        }
    }
}
