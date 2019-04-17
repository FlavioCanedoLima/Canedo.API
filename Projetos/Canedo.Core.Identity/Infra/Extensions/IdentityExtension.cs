using Canedo.Core.Identity.Application.Interfaces;
using Canedo.Core.Identity.Application.Services;
using Canedo.Core.Identity.Data;
using Canedo.Core.Identity.Domain.Interfaces;
using Canedo.Core.Identity.Domain.UserApi.Model;
using Canedo.Core.Identity.Domain.UserApi.Services;
using Canedo.Core.Identity.Infra.Configuration;
using Canedo.Core.Infra.Helpers.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Canedo.Core.Identity.Infra.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentitySqlServerConfiguration(this IServiceCollection services)
        {
            services
                .AddDbContext<CustomIdentityDbContext<IdentityUserApi>>(opition => opition.UseSqlServer(services.GetInstance<IConfiguration>().GetConnectionString("CanedoDotNetCoreApi.Identity")));
            services
                .AddIdentity<IdentityUserApi, IdentityRole>()
                .AddEntityFrameworkStores<CustomIdentityDbContext<IdentityUserApi>>()
                .AddDefaultTokenProviders();
            services
                .InitializeIdentityDatabase();
            services                
                .AddScoped<IValidateUserService, ValidateUserApiService>()
                .AddScoped<IValidateCredentialsService, ValidateCredentialsService>();

            return services;
        }

        private static IServiceCollection InitializeIdentityDatabase(this IServiceCollection services)
        {
            var context = services.GetInstance<CustomIdentityDbContext<IdentityUserApi>>();
            var userManager = services.GetInstance<UserManager<IdentityUserApi>>();
            var roleManager = services.GetInstance<RoleManager<IdentityRole>>();

            new IdentityConfiguration<IdentityUserApi>(context, userManager, roleManager).Initialize();

            return services;
        }
    }
}
