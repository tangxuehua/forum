using System.Reflection;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ENode.Commanding;
using ENode.Configurations;
using ENode.SqlServer;
using ENode.Infrastructure;
using Forum.Domain.Accounts;
using Forum.Infrastructure;
using Forum.QueryServices;

namespace Forum.Tests
{
    public abstract class TestBase
    {
        private static ENodeConfiguration _enodeConfiguration;
        protected static ICommandService _commandService;
        protected static ISectionQueryService _sectionQueryService;
        protected static IPostQueryService _postQueryService;
        protected static IReplyQueryService _replyQueryService;
        protected static ILogger _logger;

        protected static void Initialize()
        {
            if (_enodeConfiguration != null)
            {
                CleanupEnode();
            }

            ConfigSettings.Initialize();

            InitializeENode();

            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(TestBase));
            _logger.Info("ENode initialized.");

            _commandService = ObjectContainer.Resolve<ICommandService>();
            _sectionQueryService = ObjectContainer.Resolve<ISectionQueryService>();
            _postQueryService = ObjectContainer.Resolve<IPostQueryService>();
            _replyQueryService = ObjectContainer.Resolve<IReplyQueryService>();

            ObjectContainer.Resolve<ILockService>().AddLockKey(typeof(Account).Name);
        }

        protected CommandResult ExecuteCommand(ICommand command)
        {
            return _commandService.ExecuteAsync(command, CommandReturnType.EventHandled).Result.Data;
        }

        private static void InitializeENode()
        {
            var assemblies = new[]
            {
                Assembly.Load("Forum.Infrastructure"),
                Assembly.Load("Forum.Commands"),
                Assembly.Load("Forum.Domain"),
                Assembly.Load("Forum.Domain.Dapper"),
                Assembly.Load("Forum.CommandHandlers"),
                Assembly.Load("Forum.Denormalizers.Dapper"),
                Assembly.Load("Forum.ProcessManagers"),
                Assembly.Load("Forum.QueryServices"),
                Assembly.Load("Forum.QueryServices.Dapper"),
                Assembly.Load("Forum.Tests")
            };

            _enodeConfiguration = Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .CreateENode()
                .RegisterENodeComponents()
                .UseSqlServerEventStore()
                .UseSqlServerPublishedVersionStore()
                .UseSqlServerLockService()
                .RegisterBusinessComponents(assemblies)
                .InitializeEQueue()
                .UseEQueue()
                .BuildContainer()
                .InitializeSqlServerEventStore(ConfigSettings.ENodeConnectionString)
                .InitializeSqlServerPublishedVersionStore(ConfigSettings.ENodeConnectionString)
                .InitializeSqlServerLockService(ConfigSettings.ENodeConnectionString)
                .InitializeBusinessAssemblies(assemblies)
                .StartEQueue()
                .Start();
        }
        private static void CleanupEnode()
        {
            _enodeConfiguration.Stop();
            _logger.Info("ENode shutdown.");
        }
    }
}
