using System;
using System.Reflection;
using System.Threading;
using ECommon.Autofac;
using ECommon.Configurations;
using ECommon.IoC;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ENode.Commanding;
using ENode.Configurations;
using ENode.Domain;
using NUnit.Framework;

namespace Forum.Domain.Tests
{
    [TestFixture]
    public class TestBase
    {
        protected ICommandService _commandService;
        protected IMemoryCache _memoryCache;
        private const string ConnectionString = "Data Source=(local);Initial Catalog=Forum;Integrated Security=True;Connect Timeout=30;Min Pool Size=10;Max Pool Size=100";
        private static bool _initialized;

        [TestFixtureSetUp]
        public void SetUp()
        {
            if (!_initialized)
            {
                var assemblies = new[]
                {
                    Assembly.Load("Forum.Domain"),
                    Assembly.Load("Forum.CommandHandlers"),
                    Assembly.Load("Forum.Denormalizers.Dapper"),
                    Assembly.Load("Forum.Domain.Repositories.Dapper"),
                    Assembly.Load("Forum.EventSynchronizers"),
                    Assembly.Load("Forum.QueryServices"),
                    Assembly.Load("Forum.QueryServices.Dapper"),
                    Assembly.Load("Forum.Domain.Tests")
                };
                Configuration
                    .Create()
                    .UseAutofac()
                    .RegisterCommonComponents()
                    .UseLog4Net()
                    .UseJsonNet()
                    .CreateENode()
                    .RegisterENodeComponents()
                    .RegisterBusinessComponents(assemblies)
                    .UseDefaultSqlQueryDbConnectionFactory(ConnectionString)
                    .UseRegistrationDapperRepository(ConnectionString)
                    .SetProviders()
                    .UseEQueue()
                    .InitializeBusinessAssemblies(assemblies)
                    .StartRetryCommandService()
                    .StartWaitingCommandService()
                    .StartEQueue();
                _initialized = true;
            }
            _commandService = ObjectContainer.Resolve<ICommandService>();
            _memoryCache = ObjectContainer.Resolve<IMemoryCache>();
        }
    }
}
