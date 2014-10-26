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
using NUnit.Framework;

namespace Forum.Domain.Tests
{
    [TestFixture]
    public class TestBase
    {
        protected ICommandService _commandService;
        protected IMemoryCache _memoryCache;
        private static bool _initialized;
        protected static ENodeConfiguration _enodeConfiguration;

        [TestFixtureSetUp]
        public void SetUp()
        {
            if (!_initialized)
            {
                ConfigSettings.Initialize();

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

                _enodeConfiguration = Configuration
                    .Create()
                    .UseAutofac()
                    .RegisterCommonComponents()
                    .UseLog4Net()
                    .UseJsonNet()
                    .CreateENode(setting)
                    .RegisterENodeComponents()
                    .RegisterBusinessComponents(assemblies)
                    .SetProviders()
                    .UseSqlServerLockService()
                    .UseSqlServerCommandStore()
                    .UseSqlServerEventStore()
                    .UseEQueue()
                    .InitializeBusinessAssemblies(assemblies)
                    .StartENode()
                    .StartEQueue();
                _initialized = true;
            }
            _commandService = ObjectContainer.Resolve<ICommandService>();
            _memoryCache = ObjectContainer.Resolve<IMemoryCache>();

            ObjectContainer.Resolve<ILockService>().AddLockKey(typeof(Account).Name);
        }
    }
}
