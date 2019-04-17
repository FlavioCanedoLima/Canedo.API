using Canedo.Core.Identity.Application.Interfaces;
using Canedo.Core.Identity.Application.Model;
using Canedo.Core.Infra.JWT.Interfaces;
using Canedo.Core.Infra.JWT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            [FromServices]ISigningConfiguration signingConfiguration)
        {
            bool allCredentialsIsValid = false;

            if (usuario != null && !string.IsNullOrWhiteSpace(usuario.UserID))
            {
                var credentialIsValid = validateCredentialsService_.ValidateUser(usuario.UserID, usuario.Password);

                if (credentialIsValid)
                    allCredentialsIsValid = validateCredentialsService_.ValidateRole("AdminRoot");

                if (allCredentialsIsValid)
                {
                    var tokenService = new TokenService(validateCredentialsService_.GetCurrentIdentityUser(), signingConfiguration);

                    var token = tokenService.GenerateToken();

                    return new
                    {
                        authenticated = true,
                        created = tokenService.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"),
                        expiration = tokenService.DateExpire.ToString("yyyy-MM-dd HH:mm:ss"),
                        accessToken = token,
                        message = "OK"
                    };
                }
            }

            return new { authenticated = false, message = "Falha ao autenticar" };
        }
    }
}