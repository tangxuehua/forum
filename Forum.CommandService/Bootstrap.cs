using System;
using System.Reflection;
using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ECommon.Logging;
using ENode.Configurations;
using ENode.Infrastructure;
using Forum.Domain.Accounts;
using Forum.Infrastructure;
using Topshelf;

namespace Forum.CommandService
{
	public class Bootstrap : ServiceControl
	{
		private ILogger _logger;
		private Configuration _ecommonConfiguration;
		private ENodeConfiguration _enodeConfiguration;

		#region Initialize
		private void Initialize()
		{
			InitializeECommon();
			try
			{
				InitializeENode();
				InitializeCommandService();
			}
			catch (Exception ex)
			{
				_logger.Error("Initialize ENode failed.", ex);
				throw;
			}
		}

		private void InitializeECommon()
		{
			_ecommonConfiguration = Configuration
				.Create()
				.UseAutofac()
				.RegisterCommonComponents()
				.UseLog4Net()
				.UseJsonNet()
				.RegisterUnhandledExceptionHandler();
			_logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Bootstrap).FullName);
			_logger.Info("ECommon initialized.");
		}
		private void InitializeENode()
		{
			ConfigSettings.Initialize();

			var assemblies = new[]
			{
				Assembly.Load("Forum.Infrastructure"),
				Assembly.Load("Forum.Commands"),
				Assembly.Load("Forum.Domain"),
				Assembly.Load("Forum.Domain.Dapper"),
				Assembly.Load("Forum.CommandHandlers"),
				Assembly.Load("Forum.CommandService")
			};
			var setting = new ConfigurationSetting
			{
				SqlDefaultConnectionString = ConfigSettings.ENodeConnectionString
			};

			_enodeConfiguration = _ecommonConfiguration
				.CreateENode(setting)
				.RegisterENodeComponents()
				.RegisterBusinessComponents(assemblies)
				.UseSqlServerLockService()
				.UseSqlServerCommandStore()
				.UseSqlServerEventStore()
				.UseEQueue()
				.InitializeBusinessAssemblies(assemblies);
			_logger.Info("ENode initialized.");
		}
		private void InitializeCommandService()
		{
			ObjectContainer.Resolve<ILockService>().AddLockKey(typeof(Account).Name);
			_logger.Info("Command service initialized.");
		}
		#endregion

		public bool Start(HostControl hostControl)
		{
			try
			{
				Initialize();
				_enodeConfiguration.StartEQueue();
				return true;
			}
			catch (Exception ex)
			{
				_logger.Error("EQueue start failed.", ex);
				return false;
			}
		}

		public bool Stop(HostControl hostControl)
		{
			try
			{
				_enodeConfiguration.ShutdownEQueue();
				return true;
			}
			catch (Exception ex)
			{
				_logger.Error("EQueue stop failed.", ex);
				return false;
			}
		}
	}
}
