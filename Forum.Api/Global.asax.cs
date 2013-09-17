using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using ENode;
using ENode.Autofac;
using ENode.Infrastructure;
using ENode.JsonNet;
using ENode.Log4Net;
using ENode.Mongo;
using Forum.Api.Extensions;

namespace Forum.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private const string CommandSideConnectionString = "mongodb://localhost/ForumDB";
        private const string AccountRegistrationInfoCollectionName = "AccountRegistrationInfoCollection";
        private const string QuerySideConnectionString = "Data Source=(localdb)\\Projects;Initial Catalog=ForumDB;Integrated Security=True;Connect Timeout=30;Min Pool Size=10;Max Pool Size=100";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var assemblies = new[]
            {
                Assembly.Load("Forum.Domain"),
                Assembly.Load("Forum.Domain.Repositories.MongoDB"),
                Assembly.Load("Forum.CommandHandlers"),
                Assembly.Load("Forum.Denormalizers.Dapper"),
                Assembly.Load("Forum.QueryServices.Dapper")
            };
            Configuration
                .Create()
                .UseAutofac()
                .RegisterFrameworkComponents()
                .RegisterBusinessComponents(assemblies)
                .UseLog4Net()
                .UseJsonNet()
                .UseMongo(CommandSideConnectionString)
                .UseDefaultSqlQueryDbConnectionFactory(QuerySideConnectionString)
                .MongoAccountRegistrationInfoRepository(CommandSideConnectionString, AccountRegistrationInfoCollectionName)
                .CreateAllDefaultProcessors()
                .Initialize(assemblies)
                .Start();

            var container = ((AutofacObjectContainer)ObjectContainer.Current).Container;
            RegisterControllers(container);
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacContainer(container);
        }

        private static void RegisterControllers(IContainer container)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
            containerBuilder.Update(container);
        }
    }

    class AutofacContainer : IDependencyScope, System.Web.Http.Dependencies.IDependencyResolver
    {
        protected IContainer _container;

        public AutofacContainer(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;
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