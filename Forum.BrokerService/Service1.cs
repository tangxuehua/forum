using System;
using System.Net.Sockets;
using System.ServiceProcess;
using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ECommon.Logging;
using EQueue.Broker;
using EQueue.Configurations;

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
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _broker = new BrokerController(new BrokerSetting { DeleteMessageInterval = 60000 }).Initialize().Start();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        protected override void OnStop()
        {
            try
            {
                _broker.Shutdown();
            }
            catch (SocketException ex)
            {
                _logger.InfoFormat("Socket {0}.", ex.SocketErrorCode);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        static void InitializeEQueue()
        {
            Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterEQueueComponents();
        }
    }
}
