using System;
using System.Configuration;
using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ECommon.Logging;
using EQueue.Broker;
using EQueue.Configurations;
using ECommonConfiguration = ECommon.Configurations.Configuration;
using Topshelf;

namespace Forum.BrokerService
{
	public class Bootstrap : ServiceControl
	{
		private ILogger _logger;
		private ECommonConfiguration _ecommonConfiguration;
		private BrokerController _broker;

		#region Initialize
		private void Initialize()
		{
			InitializeECommon();
			try
			{
				InitializeEQueue();
			}
			catch (Exception ex)
			{
				_logger.Error("Initialize EQueue failed.", ex);
				throw;
			}
		}

		private void InitializeECommon()
		{
			_ecommonConfiguration = ECommonConfiguration
				.Create()
				.UseAutofac()
				.RegisterCommonComponents()
				.UseLog4Net()
				.UseJsonNet()
				.RegisterUnhandledExceptionHandler();
			_logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Bootstrap).FullName);
			_logger.Info("ECommon initialized.");
		}
		private void InitializeEQueue()
		{
			_ecommonConfiguration.RegisterEQueueComponents();
			var storePath = ConfigurationManager.AppSettings["equeueStorePath"];
			_broker = BrokerController.Create(new BrokerSetting(storePath));
			_logger.Info("EQueue initialized.");
		}

		#endregion

		public bool Start(HostControl hostControl)
		{
			try
			{
				Initialize();

				_broker.Start();
				return true;
			}
			catch (Exception ex)
			{
				_logger.Error("Broker start failed.", ex);
				return false;
			}
		}

		public bool Stop(HostControl hostControl)
		{
			try
			{
				if (_broker != null)
				{
					_broker.Shutdown();
				}
				return true;
			}
			catch (Exception ex)
			{
				_logger.Error("Broker stop failed.", ex);

				return false;
			}
		}
	}
}
