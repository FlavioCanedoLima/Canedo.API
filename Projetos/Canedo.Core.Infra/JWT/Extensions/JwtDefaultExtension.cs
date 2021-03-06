﻿using Canedo.Core.Infra.Helpers.Extensions;
using Canedo.Core.Infra.JWT.Configuration;
using Canedo.Core.Infra.JWT.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Canedo.Core.Infra.JWT.Extensions
{
    public static class JwtDefaultExtension
    {
        public static IServiceCollection AddAuthenticationJwtBearer(this IServiceCollection services)
        {
            var signingConfiguration = services.GetInstance<ISigningConfiguration>();
            var tokenConfiguration = services.GetInstance<TokenConfiguration>();

            services
                .AddAuthentication(authOptions =>
                {
                    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(bearerOptions =>
                {
                    var paramsValidation = bearerOptions.TokenValidationParameters;
                    paramsValidation.IssuerSigningKey = signingConfiguration.Key;
                    paramsValidation.ValidAudience = tokenConfiguration.Audience;
                    paramsValidation.ValidIssuer = tokenConfiguration.Issuer;

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
    }
}
