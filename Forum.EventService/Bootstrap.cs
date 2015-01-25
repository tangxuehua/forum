using System;
using System.Reflection;
using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ECommon.Logging;
using ENode.Configurations;
using Forum.Infrastructure;

namespace Forum.EventService
{
    public class Bootstrap
    {
        private static ILogger _logger;
        private static Configuration _ecommonConfiguration;
        private static ENodeConfiguration _enodeConfiguration;

        public static void Initialize()
        {
            InitializeECommon();
            try
            {
                InitializeENode();
            }
            catch (Exception ex)
            {
                _logger.Error("Initialize ENode failed.", ex);
                throw;
            }
        }
        public static void Start()
        {
            try
            {
                _enodeConfiguration.StartENode(NodeType.EventProcessor | NodeType.MessageProcessor).StartEQueue();
            }
            catch (Exception ex)
            {
                _logger.Error("ENode or EQueue start failed.", ex);
                throw;
            }
        }
        public static void Stop()
        {
            try
            {
                _enodeConfiguration.ShutdownEQueue();
            }
            catch (Exception ex)
            {
                _logger.Error("EQueue stop failed.", ex);
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
        private static void InitializeENode()
        {
            ConfigSettings.Initialize();

            var assemblies = new[]
            {
                Assembly.Load("Forum.Infrastructure"),
                Assembly.Load("Forum.Domain"),
                Assembly.Load("Forum.Denormalizers.Dapper"),
                Assembly.Load("Forum.EventService")
            };
            var setting = new ConfigurationSetting
            {
                SqlServerDefaultConnectionString = ConfigSettings.ConnectionString
            };

            _enodeConfiguration = _ecommonConfiguration
                .CreateENode(setting)
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseSqlServerAggregatePublishVersionStore()
                .UseSqlServerMessageHandleRecordStore()
                .UseEQueue()
                .InitializeBusinessAssemblies(assemblies);
            _logger.Info("ENode initialized.");
        }
    }
}
