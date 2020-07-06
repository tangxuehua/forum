using System.Configuration;
using System.Reflection;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ECommon.Serilog;
using ENode.Configurations;
using ENode.Infrastructure;
using ENode.SqlServer;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.CommandHost
{
    public class Bootstrap
    {
        private static ENodeConfiguration _enodeConfiguration;

        public static void Initialize()
        {
            InitializeENode();
            InitializeCommandService();
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
                Assembly.Load("Forum.Commands"),
                Assembly.Load("Forum.Domain"),
                Assembly.Load("Forum.Domain.Dapper"),
                Assembly.Load("Forum.CommandHandlers"),
                Assembly.Load("Forum.ProcessManagers"),
                Assembly.Load("Forum.CommandHost")
            };

            var loggerFactory = new SerilogLoggerFactory()
                .AddFileLogger("ECommon", "logs\\ecommon")
                .AddFileLogger("EQueue", "logs\\equeue")
                .AddFileLogger("ENode", "logs\\enode", minimumLevel: Serilog.Events.LogEventLevel.Debug)
                .AddFileLogger("ENode.EQueue", "logs\\enode.equeue", minimumLevel: Serilog.Events.LogEventLevel.Debug);

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
                .UseSqlServerEventStore()
                .UseSqlServerLockService()
                .UseEQueue()
                .BuildContainer()
                .InitializeSqlServerEventStore(ConfigSettings.ENodeConnectionString)
                .InitializeSqlServerLockService(ConfigSettings.ENodeConnectionString)
                .InitializeBusinessAssemblies(assemblies);
        }
        private static void InitializeCommandService()
        {
            ObjectContainer.Resolve<ILockService>().AddLockKey(typeof(Account).Name);
            ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Program)).Info("Command Host initialized.");
        }
    }
}
