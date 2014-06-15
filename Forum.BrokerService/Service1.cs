using System;
using System.ServiceProcess;
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
    public partial class Service1 : ServiceBase
    {
        private ILogger _logger;
        private BrokerController _broker;

        public Service1()
        {
            InitializeComponent();
            InitializeEQueue();
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create("Forum.BrokerService");
            _logger.Info("Service initialized.");
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _broker.Start();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        protected override void OnStop()
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
                _logger.Error(ex);
                throw;
            }
        }

        private void InitializeEQueue()
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

            Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterEQueueComponents()
                .UseSqlServerMessageStore(messageStoreSetting)
                .UseSqlServerOffsetManager(offsetManagerSetting);

            var setting = new BrokerSetting { SuspendPullRequestMilliseconds = 3000 };
            _broker = new BrokerController(setting);
        }
    }
}
