using Autofac.Extensions.DependencyInjection;
using ECommon.Autofac;
using ECommon.Components;
using ENode.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Web
{
    public static class ConfigurationExtensions
    {
        /// <summary>Register all the mvc services into ioc container.
        /// </summary>
        /// <returns></returns>
        public static ENodeConfiguration RegisterMvcServices(this ENodeConfiguration configuration, IServiceCollection services)
        {
            var containerBuilder = (ObjectContainer.Current as AutofacObjectContainer).ContainerBuilder;
            services.AddMvc();
            containerBuilder.Populate(services);
            return configuration;
        }
    }
}