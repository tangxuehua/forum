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
using Forum.QueryServices;
using NUnit.Framework;

namespace Forum.Domain.Tests
{
    [TestFixture]
    public class TestBase
    {
        protected ICommandService _commandService;
        protected ISectionQueryService _sectionQueryService;
        protected IPostQueryService _postQueryService;
        protected IReplyQueryService _replyQueryService;
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
                    .UseSqlServerLockService()
                    .UseSqlServerCommandStore()
                    .UseSqlServerEventStore()
                    .UseEQueue()
                    .InitializeBusinessAssemblies(assemblies)
                    .StartENode(NodeType.CommandProcessor | NodeType.EventProcessor)
                    .StartEQueue();
                _initialized = true;
            }
            _commandService = ObjectContainer.Resolve<ICommandService>();
            _sectionQueryService = ObjectContainer.Resolve<ISectionQueryService>();
            _postQueryService = ObjectContainer.Resolve<IPostQueryService>();
            _replyQueryService = ObjectContainer.Resolve<IReplyQueryService>();

            ObjectContainer.Resolve<ILockService>().AddLockKey(typeof(Account).Name);
        }
    }
}
