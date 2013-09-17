using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using ENode;
using ENode.Autofac;
using ENode.Infrastructure;
using ENode.JsonNet;
using ENode.Log4Net;
using ENode.Mongo;
using Forum.Web.Extensions;

namespace Forum.Web
{
    public class MvcApplication : HttpApplication
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
            AuthConfig.RegisterAuth();

            var assemblies = new[]
            {
                Assembly.Load("Forum.Domain"),
                Assembly.Load("Forum.Domain.Repositories.MongoDB"),
                Assembly.Load("Forum.CommandHandlers"),
                Assembly.Load("Forum.Denormalizers.Dapper"),
                Assembly.Load("Forum.QueryServices.Dapper"),
                Assembly.Load("Forum.Web")
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
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterControllers(IContainer container)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterControllers(typeof(MvcApplication).Assembly);
            containerBuilder.Update(container);
        }
    }
}