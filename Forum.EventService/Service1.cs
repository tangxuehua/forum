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
using Forum.Infrastructure;

namespace Forum.EventService
{
    public partial class Service1 : ServiceBase
    {
        private ILogger _logger;
        private ENodeConfiguration _configuration;

        public Service1()
        {
            InitializeComponent();
            InitializeENode();
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(GetType().FullName);
            _logger.Info("Service initialized.");
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _configuration.StartEQueue();
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
                _configuration.ShutdownEQueue();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        private void InitializeENode()
        {
            ConfigSettings.Initialize();

            var assemblies = new[]
            {
                Assembly.Load("Forum.Infrastructure"),
                Assembly.Load("Forum.Domain"),
                Assembly.Load("Forum.Denormalizers.Dapper")
            };

            _configuration = Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .CreateENode()
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseSqlServerEventPublishInfoStore(ConfigSettings.ConnectionString)
                .UseSqlServerEventHandleInfoStore(ConfigSettings.ConnectionString)
                .SetProviders()
                .UseEQueue()
                .InitializeBusinessAssemblies(assemblies);
        }
    }
}
