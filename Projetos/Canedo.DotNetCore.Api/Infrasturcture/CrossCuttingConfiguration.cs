using Canedo.DotNetCore.Data.Configuration;
using Canedo.DotNetCore.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Canedo.DotNetCore.Api.Infrasturcture
{
    public static class CrossCuttingConfiguration
    {
        public static void AddCrossCuttingService(this IServiceCollection services)
        {
            services.AddTransient<RavenConfig>();
            services.AddTransient<UserRepository>();
        }
    }
}
