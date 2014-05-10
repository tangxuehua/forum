using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
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
        }

        private static void RegisterControllers()
        {
            var container = (ObjectContainer.Current as AutofacObjectContainer).Container;
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.Update(container);
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
