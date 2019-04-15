using Microsoft.Extensions.DependencyInjection;

namespace Canedo.Core.Infra.Helpers.Extensions
{
    public static class DependencyInjectorExtension
    {
        public static T GetInstance<T>(this IServiceCollection services) where T : class
        {
            return services.BuildServiceProvider().GetService<T>();
        }
    }
}
