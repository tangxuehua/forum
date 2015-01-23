using System.Reflection;
using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ENode.Commanding;
using ENode.Configurations;
using ENode.Domain;
using ENode.Infrastructure;
using Forum.Domain.Accounts;
using Forum.Infrastructure;

namespace Forum.Domain.Tests
{
    public abstract class TestBase
    {
        protected static ICommandService _commandService;
        protected static IMemoryCache _memoryCache;

        static TestBase()
        {
            ConfigSettings.Initialize();
            InitializeENode();
            _commandService = ObjectContainer.Resolve<ICommandService>();
            _memoryCache = ObjectContainer.Resolve<IMemoryCache>();
            ObjectContainer.Resolve<ILockService>().AddLockKey(typeof(Account).Name);
        }

        private static void InitializeENode()
        {
            var assemblies = new[]
            {
                Assembly.Load("Forum.Infrastructure"),
                Assembly.Load("Forum.Domain"),
                Assembly.Load("Forum.Domain.Dapper"),
                Assembly.Load("Forum.CommandHandlers"),
                Assembly.Load("Forum.Denormalizers.Dapper"),
                Assembly.Load("Forum.QueryServices"),
                Assembly.Load("Forum.QueryServices.Dapper"),
                Assembly.Load("Forum.Domain.Tests")
            };

            var setting = new ConfigurationSetting
            {
                SqlServerDefaultConnectionString = ConfigSettings.ConnectionString
            };

            Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .CreateENode(setting)
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseSqlServerLockService()
                .UseSqlServerCommandStore()
                .UseSqlServerEventStore()
                .UseEQueue()
                .InitializeBusinessAssemblies(assemblies)
                .StartENode(NodeType.CommandProcessor | NodeType.EventProcessor)
                .StartEQueue();
        }
    }
}
