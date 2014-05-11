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

namespace Forum.EventService
{
    public partial class Service1 : ServiceBase
    {
        private ILogger _logger;
        private ENodeConfiguration _enodeConfiguration;

        public Service1()
        {
            InitializeComponent();
            InitializeENode();
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create("Forum.EventService");
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _enodeConfiguration.StartEQueue();
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
                _enodeConfiguration.ShutdownEQueue();
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

        private void InitializeENode()
        {
            ConfigSettings.ConnectionString = "Data Source=(local);Initial Catalog=Forum;uid=sa;pwd=howareyou;Connect Timeout=30;Min Pool Size=10;Max Pool Size=100";

            var assemblies = new[]
            {
                Assembly.Load("Forum.Domain"),
                Assembly.Load("Forum.Denormalizers.Dapper")
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
