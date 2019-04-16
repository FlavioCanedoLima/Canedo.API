using Canedo.Core.Infra.Helpers.Extensions;
using Canedo.Core.Infra.JWT.Configuration;
using Canedo.Core.Infra.JWT.Interfaces;
using Canedo.Core.Infra.JWT.Services;
using Canedo.DotNetCore.Api.Infra.AppSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Canedo.DotNetCore.Api.Infra
{
    public static class CustomConfiguration
    {
        #region ..:: ConfigureServices ::..

        public static IServiceCollection AddCustomConfigurations(this IServiceCollection services)
        {
            services.AddAppSettings();
            services.AddCrossCuttingService();

            return services;
        }

        private static IServiceCollection AddAppSettings(this IServiceCollection services)
        {
            var configuration = services.GetInstance<IConfiguration>();

            services.AddSingleton(
                new AppSetting()
                {
                    Logging = configuration.GetSection("Logging").Configure<Logging>(),
                    AllowedHosts = configuration.GetValue<string>("AllowedHosts"),
                    ConnectionStrings = configuration.GetSection("ConnectionStrings").Configure<ConnectionStrings>(),
                    Ravendb = configuration.GetSection("RavenDB").Configure<Ravendb>()
                });

            return services;
        }

        private static IServiceCollection AddCrossCuttingService(this IServiceCollection services)
        {
            services.AddSingleton<ISigningConfiguration>(new SigningConfiguration(services.GetInstance<IConfiguration>().GetSection("TokenConfigurations").Configure<TokenConfiguration>()));
            services.AddSingleton<TokenService>();

            return services;
        }        

        #endregion
    }
}
