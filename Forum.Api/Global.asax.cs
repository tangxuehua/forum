using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Autofac;
using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ENode.Configurations;
using Forum.Api.Extensions;
using Forum.Infrastructure;

namespace Forum.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private const string ConnectionString = "Data Source=(local);Initial Catalog=Forum;Integrated Security=True;Connect Timeout=30;Min Pool Size=10;Max Pool Size=100";

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            ConfigSettings.ConnectionString = ConnectionString;

            var assemblies = new[]
                {
                    Assembly.Load("Forum.Domain"),
                    Assembly.Load("Forum.CommandHandlers"),
                    Assembly.Load("Forum.Denormalizers.Dapper"),
                    Assembly.Load("Forum.Domain.Repositories.Dapper"),
                    Assembly.Load("Forum.EventSynchronizers"),
                    Assembly.Load("Forum.QueryServices"),
                    Assembly.Load("Forum.QueryServices.Dapper")
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
                .UseSqlServerEventStore(ConnectionString)
                .SetProviders()
                .UseEQueue()
                .InitializeBusinessAssemblies(assemblies)
                .StartRetryCommandService()
                .StartWaitingCommandService()
                .StartEQueue();

            RegisterControllers();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacContainer();
        }

        private static void RegisterControllers()
        {
            foreach (var controllerType in typeof(WebApiApplication).Assembly.GetTypes().Where(x => x.Name.EndsWith("Controller")))
            {
                ObjectContainer.RegisterType(controllerType, LifeStyle.Transient);
            }
        }

        class AutofacContainer : IDependencyScope, IDependencyResolver
        {
            private IContainer _container;

            public AutofacContainer()
            {
                _container = ((AutofacObjectContainer)ObjectContainer.Current).Container;
            }

            public IDependencyScope BeginScope()
            {
                return this;
            }
            public object GetService(Type serviceType)
            {
                if (_container.IsRegistered(serviceType))
                {
                    return _container.Resolve(serviceType);
                }
                else
                {
                    return null;
                }
            }
            public IEnumerable<object> GetServices(Type serviceType)
            {
                if (_container.IsRegistered(serviceType))
                {
                    return new[] { _container.Resolve(serviceType) };
                }
                else
                {
                    return new List<object>();
                }
            }
            public void Dispose()
            {
            }
        }
    }
}
