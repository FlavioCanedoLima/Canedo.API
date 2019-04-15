using Canedo.Core.Infra.Helpers.Extensions;
using Canedo.DotNetCore.Api.Infra.AppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Canedo.DotNetCore.Api.Infra
{
    public static class CustomConfiguration
    {
        #region ..:: ConfigureServices ::..

        public static IServiceCollection AddAppSettings(this IServiceCollection services)
        {
            var configuration = services.GetInstance<IConfiguration>();

            services.AddSingleton(
                new AppSetting()
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
            var appSettings = services.GetInstance<AppSetting>();

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
            //services.AddTransient<RavenConfig>();
            //services.AddTransient<UserRepository>();
            

            return services;
        }        

        #endregion
    }
}
