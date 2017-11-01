using System;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ENode.Configurations;
using Forum.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ConfigSettings.Initialize();
            InitializeENode(services);
            return new AutofacServiceProvider(((AutofacObjectContainer)ObjectContainer.Current).Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeENode(IServiceCollection services)
        {
            var assemblies = new[]
            {
                Assembly.Load("Forum.Commands"),
                Assembly.Load("Forum.QueryServices"),
                Assembly.Load("Forum.QueryServices.Dapper"),
                Assembly.Load("Forum.Web")
            };

            ECommon.Configurations.Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .CreateENode()
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseEQueue()
                .RegisterMvcServices(services)
                .BuildContainer()
                .InitializeBusinessAssemblies(assemblies)
                .StartEQueue();

            ObjectContainer.Resolve<ILoggerFactory>().Create(GetType().FullName).Info("ENode initialized.");
        }
    }
}
