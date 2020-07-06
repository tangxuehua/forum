using System;
using System.Reflection;
using Autofac;
using ECommon.Configurations;
using ECommon.Serilog;
using ENode.Configurations;
using Forum.Infrastructure;
using Forum.Web.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Forum.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAntiforgery();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Account/Login";
                options.SlidingExpiration = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            ConfigSettings.ForumConnectionString = Configuration.GetConnectionString("forum");
            ConfigSettings.ENodeConnectionString = Configuration.GetConnectionString("enode");
            var assemblies = new[]
            {
                Assembly.Load("Forum.Commands"),
                Assembly.Load("Forum.QueryServices"),
                Assembly.Load("Forum.QueryServices.Dapper"),
                Assembly.Load("Forum.Web")
            };
            ENodeConfiguration
                .Instance
                .InitializeBusinessAssemblies(assemblies)
                .StartEQueue();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            InitializeENode(builder);
        }

        private void InitializeENode(ContainerBuilder builder)
        {
            var assemblies = new[]
            {
                Assembly.Load("Forum.Commands"),
                Assembly.Load("Forum.QueryServices"),
                Assembly.Load("Forum.QueryServices.Dapper"),
                Assembly.Load("Forum.Web")
            };
            var loggerFactory = new SerilogLoggerFactory()
                .AddFileLogger("ECommon", "logs\\ecommon")
                .AddFileLogger("EQueue", "logs\\equeue")
                .AddFileLogger("ENode", "logs\\enode", minimumLevel: Serilog.Events.LogEventLevel.Debug);

            ECommon.Configurations.Configuration
                .Create()
                .UseAutofac(builder)
                .RegisterCommonComponents()
                .UseSerilog(loggerFactory)
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .CreateENode()
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .UseEQueue();
        }
    }
}
