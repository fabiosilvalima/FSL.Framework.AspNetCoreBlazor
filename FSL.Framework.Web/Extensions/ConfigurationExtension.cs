using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace FSL.Framework.Web.Extensions
{
    public static class ConfigurationExtension
    {
        public static void AddConfiguration<T>(
            this IServiceCollection services,
            IConfiguration configuration,
            T instance = default,
            string configurationTag = null)
            where T : class
        {
            if (string.IsNullOrEmpty(configurationTag))
            {
                configurationTag = typeof(T).Name;
            }

            instance ??= CreateInstance<T>(configuration);
            new ConfigureFromConfigurationOptions<T>(configuration.GetSection(configurationTag)).Configure(instance);
            services.AddSingleton(instance);
        }

        private static T CreateInstance<T>(
            IConfiguration configuration) 
            where T : class
        {
            try
            {
                return (T)Activator.CreateInstance(typeof(T), configuration);
            }
            catch
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
        }
    }
}
