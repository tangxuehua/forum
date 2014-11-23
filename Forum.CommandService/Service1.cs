using System;
using System.Reflection;
using System.ServiceProcess;
using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ECommon.Logging;
using ENode.Configurations;
using ENode.Infrastructure;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.CommandService
{
    public partial class Service1 : ServiceBase
    {
        private ILogger _logger;
        private Configuration _ecommonConfiguration;
        private ENodeConfiguration _enodeConfiguration;

        public Service1()
        {
            InitializeComponent();
            InitializeECommon();

            try
            {
                InitializeENode();
                InitializeCommandService();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _enodeConfiguration.StartENode(NodeType.CommandProcessor).StartEQueue();
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
                _enodeConfiguration.ShutdownEQueue();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        private void InitializeECommon()
        {
            _ecommonConfiguration = Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet();
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(GetType().FullName);
            _logger.Info("ECommon initialized.");
        }
        private void InitializeENode()
        {
            ConfigSettings.Initialize();

            var assemblies = new[]
            {
                Assembly.Load("Forum.Infrastructure"),
                Assembly.Load("Forum.Domain"),
                Assembly.Load("Forum.Domain.Dapper"),
                Assembly.Load("Forum.CommandHandlers"),
                Assembly.Load("Forum.CommandService")
            };
            var setting = new ConfigurationSetting
            {
                SqlServerDefaultConnectionString = ConfigSettings.ConnectionString
            };

            _enodeConfiguration = _ecommonConfiguration
                .CreateENode(setting)
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseSqlServerLockService()
                .UseSqlServerCommandStore()
                .UseSqlServerEventStore()
                .UseSqlServerEventPublishInfoStore()
                .UseSqlServerEventHandleInfoStore()
                .UseEQueue()
                .InitializeBusinessAssemblies(assemblies);
            _logger.Info("ENode initialized.");
        }
        private void InitializeCommandService()
        {
            ObjectContainer.Resolve<ILockService>().AddLockKey(typeof(Account).Name);
            _logger.Info("Command service initialized.");
        }
    }
}
