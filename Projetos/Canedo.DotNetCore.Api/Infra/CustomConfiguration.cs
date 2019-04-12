using Canedo.Domain.Application.User.Data;
using Canedo.Domain.Application.User.Domain.ApplicationUserApi;
using Canedo.Domain.Application.User.Infra.Configuration;
using Canedo.Domain.Core.Models.AppSettings;
using Canedo.DotNetCore.Data.Configuration;
using Canedo.DotNetCore.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Xml;


namespace Canedo.DotNetCore.Api.Infra
{
    public static class CustomConfiguration
    {
        #region ..:: ConfigureServices ::..

        private static T Configure<T>(this IConfigurationSection configurationSection) where T : class, new()
        {
            var instance = Activator.CreateInstance<T>();

            new ConfigureFromConfigurationOptions<T>(configurationSection).Configure(instance);

            return instance;
        }

        private static T GetInstance<T>(this IServiceCollection services) where T : class
        {
            return services.BuildServiceProvider().GetService<T>();
        }

        public static IServiceCollection AddAppSettings(this IServiceCollection services)
        {
            var configuration = services.GetInstance<IConfiguration>();

            services.AddSingleton(
                new AppSettings()
                {
                    Logging = configuration.GetSection("Logging").Configure<Logging>(),
                    AllowedHosts = configuration.GetValue<string>("AllowedHosts"),
                    ConnectionStrings = configuration.GetSection("ConnectionStrings").Configure<ConnectionStrings>(),
                    TokenConfigurations = configuration.GetSection("TokenConfigurations").Configure<TokenConfigurations>(),
                    Ravendb = configuration.GetSection("RavenDB").Configure<Ravendb>()
                });

            return services;
        }

        public static IServiceCollection AddAuthenticationJwtBearer(this IServiceCollection services)
        {
            var appSettings = services.GetInstance<AppSettings>();

            var signingConfigurations = new SigningConfiguration();
            services.AddSingleton(signingConfigurations);

            services
                .AddAuthentication(authOptions =>
                {
                    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(bearerOptions =>
                {
                    var paramsValidation = bearerOptions.TokenValidationParameters;
                    paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                    paramsValidation.ValidAudience = appSettings.TokenConfigurations.Audience;
                    paramsValidation.ValidIssuer = appSettings.TokenConfigurations.Issuer;

                    // Valida a assinatura de um token recebido
                    paramsValidation.ValidateIssuerSigningKey = true;

                    // Verifica se um token recebido ainda é válido
                    paramsValidation.ValidateLifetime = true;

                    /*
                     * Tempo de tolerância para a expiração de um token (utilizado
                     * caso haja problemas de sincronismo de horário entre diferentes
                     * computadores envolvidos no processo de comunicação)
                     */
                    paramsValidation.ClockSkew = TimeSpan.Zero;
                });

            /*
             * Ativa o uso do token como forma de autorizar o acesso
             * a recursos deste projeto
            */
            services.AddAuthorization(auth =>
            {
                auth
                .AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
            });

            return services;
        }

        public static IServiceCollection AddCrossCuttingService(this IServiceCollection services)
        {
            services.AddTransient<RavenConfig>();
            services.AddTransient<UserRepository>();

            return services;
        }

        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services
                .AddDbContext<CustomIdentityDbContext<ApplicationUserApi>>(opition => opition.UseSqlServer(services.GetInstance<IConfiguration>().GetConnectionString("CanedoDotNetCoreApi.Identity")));
            services
                .AddIdentity<ApplicationUserApi, IdentityRole>()
                .AddEntityFrameworkStores<CustomIdentityDbContext<ApplicationUserApi>>()
                .AddDefaultTokenProviders();
            services
                .InitializeIdentityDatabase();

            return services;
        }

        private static IServiceCollection InitializeIdentityDatabase(this IServiceCollection services)
        {
            var context = services.GetInstance<CustomIdentityDbContext<ApplicationUserApi>>();
            var userManager = services.GetInstance<UserManager<ApplicationUserApi>>();
            var roleManager = services.GetInstance<RoleManager<IdentityRole>>();

            new IdentityConfiguration<ApplicationUserApi>(context, userManager, roleManager).Initialize();

            return services;
        }

        #endregion
        #region ..:: Configure ::..

        public static IHostingEnvironment UseUserSecrets(this IHostingEnvironment environment)
        {
            XmlDocument xmldoc = new XmlDocument();

            xmldoc.Load($@"{((PhysicalFileProvider)environment.ContentRootFileProvider).Root}{environment.ApplicationName}.csproj");

            var userSecretId = xmldoc.SelectSingleNode("//UserSecretsId").InnerText;

            if (userSecretId.Any() == false) return environment;

            new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets(userSecretId)
                .Build();

            return environment;
        }

        #endregion
    }
}
