using System;
using System.Reflection;
using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ECommon.Logging;
using ENode.Configurations;
using Forum.Infrastructure;
using Topshelf;

namespace Forum.EventService
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
				Assembly.Load("Forum.Denormalizers.Dapper"),
				Assembly.Load("Forum.ProcessManagers"),
				Assembly.Load("Forum.EventService")
			};
			var setting = new ConfigurationSetting
			{
				SqlDefaultConnectionString = ConfigSettings.ENodeConnectionString
			};

			_enodeConfiguration = _ecommonConfiguration
				.CreateENode(setting)
				.RegisterENodeComponents()
				.RegisterBusinessComponents(assemblies)
				.UseSqlServerSequenceMessagePublishedVersionStore()
				.UseSqlServerMessageHandleRecordStore()
				.UseEQueue()
				.InitializeBusinessAssemblies(assemblies);
			_logger.Info("ENode initialized.");
		}
		#endregion Initialize

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
