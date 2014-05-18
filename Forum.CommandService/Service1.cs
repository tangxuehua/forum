using System;
using System.Net.Sockets;
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

namespace Forum.CommandService
{
    public partial class Service1 : ServiceBase
    {
        private ILogger _logger;
        private ENodeConfiguration _enodeConfiguration;

        public Service1()
        {
            InitializeComponent();
            InitializeENode();
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create("Forum.CommandService");
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _enodeConfiguration.StartRetryCommandService().StartWaitingCommandService().StartEQueue();
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

        private void InitializeENode()
        {
            ConfigSettings.Initialize();

            var assemblies = new[]
            {
                Assembly.Load("Forum.Domain"),
                Assembly.Load("Forum.CommandHandlers"),
                Assembly.Load("Forum.Domain.Repositories.Dapper"),
                Assembly.Load("Forum.EventSynchronizers")
            };

            _enodeConfiguration = Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .CreateENode()
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseSqlServerEventStore(ConfigSettings.ConnectionString)
                .SetProviders()
                .UseEQueue()
                .InitializeBusinessAssemblies(assemblies);
        }
    }
}
