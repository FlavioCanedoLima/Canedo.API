using Canedo.Core.Identity.Application.Interfaces;
using Canedo.Core.Identity.Application.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

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
            [FromBody]ApplicationUser usuario
            //[FromServices]UserRepository usersDAO,
            //[FromServices]SigningConfiguration signingConfigurations,
            //[FromServices]AppSettings appSettings
            )
        {
            bool credenciaisValidas = false;

            if (usuario != null && !String.IsNullOrWhiteSpace(usuario.UserID))
            {
                var credentialIsValid = validateCredentialsService_.ValidateUser(usuario.UserID, usuario.Password);
            }

                //if (usuario != null && !string.IsNullOrWhiteSpace(usuario.UserId))
                //{
                //    var usuarioBase = usersDAO.GetUser(usuario.UserId);
                //    credenciaisValidas = (usuarioBase != null &&
                //        usuario.UserId == usuarioBase.UserId &&
                //        usuario.AccessKey == usuarioBase.AccessKey);
                //}

                //if (credenciaisValidas)
                //{
                //    ClaimsIdentity identity = new ClaimsIdentity(
                //        new GenericIdentity(usuario.UserId, "Login"),
                //        new[] {
                //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                //            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserId)
                //        }
                //    );

                //    DateTime dataCriacao = DateTime.Now;
                //    DateTime dataExpiracao = dataCriacao +
                //        TimeSpan.FromSeconds(appSettings.TokenConfigurations.Seconds);

                //    var handler = new JwtSecurityTokenHandler();
                //    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                //    {
                //        Issuer = appSettings.TokenConfigurations.Issuer,
                //        Audience = appSettings.TokenConfigurations.Audience,
                //        SigningCredentials = signingConfigurations.SigningCredentials,
                //        Subject = identity,
                //        NotBefore = dataCriacao,
                //        Expires = dataExpiracao
                //    });
                //    var token = handler.WriteToken(securityToken);

                //    return new
                //    {
                //        authenticated = true,
                //        created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                //        expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                //        accessToken = token,
                //        message = "OK"
                //    };
                //}
                //else
                //{
                //    return new
                //    {
                //        authenticated = false,
                //        message = "Falha ao autenticar"
                //    };
                //}

                return null;
        }
    }

}