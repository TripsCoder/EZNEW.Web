using MicBeach.Util.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Web.DI
{
    public static class ServiceProviderConfig
    {
        /// <summary>
        /// Service Provider
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Service Collection
        /// </summary>
        public static IServiceCollection Services { get; private set; }

        /// <summary>
        /// Register Service Method
        /// </summary>
        public static Action RegisterServiceMethod { get; set; }

        /// <summary>
        /// Register Services
        /// </summary>
        /// <param name="services">services</param>
        public static void RegisterServices(IServiceCollection services)
        {
            if (services == null)
            {
                return;
            }
            Services = services;
            ServiceProvider = services.BuildServiceProvider();
            RegisterServiceMethod?.Invoke();
            ServiceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Build ServiceProvider
        /// </summary>
        public static IServiceProvider BuildServiceProvider()
        {
            IServiceCollection newServices = new ServiceCollection();
            if (Services != null)
            {
                foreach (var service in Services)
                {
                    newServices.Add(service);
                }
            }
            Services = newServices;
            ServiceProvider = newServices.BuildServiceProvider();
            return ServiceProvider;
        }
    }
}
