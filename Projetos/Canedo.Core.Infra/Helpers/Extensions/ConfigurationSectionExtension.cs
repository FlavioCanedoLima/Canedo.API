using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Canedo.Core.Infra.Helpers.Extensions
{
    public static class ConfigurationSectionExtension
    {
        public static T Configure<T>(this IConfigurationSection configurationSection) where T : class, new()
        {
            var instance = Activator.CreateInstance<T>();

            new ConfigureFromConfigurationOptions<T>(configurationSection).Configure(instance);

            return instance;
        }
    }
}
