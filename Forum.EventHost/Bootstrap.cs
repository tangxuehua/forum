using System.Configuration;
using System.Reflection;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ECommon.Serilog;
using ENode.Configurations;
using ENode.SqlServer;
using Forum.Infrastructure;

namespace Forum.EventHost
{
    public class Bootstrap
    {
        private static ENodeConfiguration _enodeConfiguration;

        public static void Initialize()
        {
            InitializeENode();
        }
        public static void Start()
        {
            _enodeConfiguration.StartEQueue().Start();
        }
        public static void Stop()
        {
            _enodeConfiguration.ShutdownEQueue().Stop();
        }

        private static void InitializeENode()
        {
            ConfigSettings.ForumConnectionString = ConfigurationManager.ConnectionStrings["forum"].ConnectionString;
            ConfigSettings.ENodeConnectionString = ConfigurationManager.ConnectionStrings["enode"].ConnectionString;

            var assemblies = new[]
            {
                Assembly.Load("Forum.Infrastructure"),
                Assembly.Load("Forum.Domain"),
                Assembly.Load("Forum.Denormalizers.Dapper"),
                Assembly.Load("Forum.EventHost")
            };

            var loggerFactory = new SerilogLoggerFactory()
                .AddFileLogger("ECommon", "logs\\ecommon")
                .AddFileLogger("EQueue", "logs\\equeue")
                .AddFileLogger("ENode", "logs\\enode", minimumLevel: Serilog.Events.LogEventLevel.Debug);

            _enodeConfiguration = ECommon.Configurations.Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseSerilog(loggerFactory)
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .CreateENode()
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseSqlServerPublishedVersionStore()
                .UseEQueue()
                .BuildContainer()
                .InitializeSqlServerPublishedVersionStore(ConfigSettings.ENodeConnectionString)
                .InitializeBusinessAssemblies(assemblies);

            ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Program)).Info("Event Host initialized.");
        }
    }
}
