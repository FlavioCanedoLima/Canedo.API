using Canedo.Core.Infra.JWT.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Canedo.Core.Infra.JWT.Services
{
    public class TokenService
    {
        readonly IdentityUser identityUser_;
        readonly ISigningConfiguration signingConfigurations_;

        public TokenService(IdentityUser identityUser, ISigningConfiguration signingConfigurations)
        {
            identityUser_ = identityUser;
            signingConfigurations_ = signingConfigurations;
        }

        public DateTime DateCreated { get; private set; }
        public DateTime DateExpire { get; private set; }

        public string GenerateToken()
        {
            var identity =
                new ClaimsIdentity(
                    new GenericIdentity(identityUser_.UserName, "Login"),
                    new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, identityUser_.UserName)
                    });

            DateCreated = DateTime.Now;
            DateExpire = DateCreated + TimeSpan.FromSeconds(signingConfigurations_.TokenConfiguration.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = signingConfigurations_.TokenConfiguration.Issuer,
                Audience = signingConfigurations_.TokenConfiguration.Audience,
                SigningCredentials = signingConfigurations_.SigningCredentials,
                Subject = identity,
                NotBefore = DateCreated,
                Expires = DateExpire
            });

            return handler.WriteToken(securityToken);
        }
    }
}
