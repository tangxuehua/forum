using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Extensions;
using ECommon.Logging;
using ECommon.Serilog;
using EQueue.Broker;
using EQueue.Configurations;
using Forum.Infrastructure;
using ECommonConfiguration = ECommon.Configurations.Configuration;

namespace Forum.BrokerHost
{
    public class Bootstrap
    {
        private static BrokerController _broker;

        public static void Initialize()
        {
            InitializeEQueue();
        }
        public static void Start()
        {
            _broker.Start();
        }
        public static void Stop()
        {
            if (_broker != null)
            {
                _broker.Shutdown();
            }
        }

        private static void InitializeEQueue()
        {
            var loggerFactory = new SerilogLoggerFactory()
                .AddFileLogger("ECommon", "logs\\ecommon")
                .AddFileLogger("EQueue", "logs\\equeue")
                .AddFileLogger("ENode", "logs\\enode");

            ECommonConfiguration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseSerilog(loggerFactory)
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .RegisterEQueueComponents()
                .BuildContainer();

            var storePath = ConfigurationManager.AppSettings["equeueStorePath"];
            var nameServerEndpoint = new IPEndPoint(IPAddress.Loopback, ConfigSettings.NameServerPort);
            var nameServerEndpoints = new List<IPEndPoint> { nameServerEndpoint };
            var brokerSetting = new BrokerSetting(false, storePath)
            {
                NameServerList = nameServerEndpoints
            };
            brokerSetting.BrokerInfo.ProducerAddress = new IPEndPoint(IPAddress.Loopback, ConfigSettings.BrokerProducerPort).ToAddress();
            brokerSetting.BrokerInfo.ConsumerAddress = new IPEndPoint(IPAddress.Loopback, ConfigSettings.BrokerConsumerPort).ToAddress();
            brokerSetting.BrokerInfo.AdminAddress = new IPEndPoint(IPAddress.Loopback, ConfigSettings.BrokerAdminPort).ToAddress();
            _broker = BrokerController.Create(brokerSetting);
            ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Bootstrap).FullName).Info("Broker initialized.");
        }
    }
}
