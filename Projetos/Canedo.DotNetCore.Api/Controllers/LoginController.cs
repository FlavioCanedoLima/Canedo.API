using Canedo.Core.Identity.Application.Interfaces;
using Canedo.Core.Identity.Application.Model;
using Canedo.DotNetCore.Api.Infra;
using Canedo.DotNetCore.Api.Infra.AppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Canedo.DotNetCore.Api.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        readonly IValidateCredentialsService validateCredentialsService_;


        public LoginController(IValidateCredentialsService validateCredentialsService)
        {
            validateCredentialsService_ = validateCredentialsService;
        }


        [AllowAnonymous]
        [HttpPost]
        public object Post(
            [FromBody]ApplicationUser usuario,
            //[FromServices]UserRepository usersDAO,
            [FromServices]SigningConfiguration signingConfigurations,
            [FromServices]AppSetting appSetting
            )
        {
            bool credenciaisValidas = false;

            if (usuario != null && !String.IsNullOrWhiteSpace(usuario.UserID))
            {
                var credentialIsValid = validateCredentialsService_.ValidateUser(usuario.UserID, usuario.Password);

                if (credentialIsValid)
                    credenciaisValidas = validateCredentialsService_.ValidateRole("Acesso-APIAlturas");

                if (credenciaisValidas)
                {
                    ClaimsIdentity identity = 
                        new ClaimsIdentity(
                            new GenericIdentity(usuario.UserID, "Login"),
                            new[] 
                            {
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserID)
                            });

                    DateTime dataCriacao = DateTime.Now;
                    DateTime dataExpiracao = dataCriacao +
                        TimeSpan.FromSeconds(appSetting.TokenConfigurations.Seconds);

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                    {
                        Issuer = appSetting.TokenConfigurations.Issuer,
                        Audience = appSetting.TokenConfigurations.Audience,
                        SigningCredentials = signingConfigurations.SigningCredentials,
                        Subject = identity,
                        NotBefore = dataCriacao,
                        Expires = dataExpiracao
                    });
                    var token = handler.WriteToken(securityToken);

                    return new
                    {
                        authenticated = true,
                        created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                        expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                        accessToken = token,
                        message = "OK"
                    };
                }
            }

            return new { authenticated = false, message = "Falha ao autenticar" };
        }
    }

}