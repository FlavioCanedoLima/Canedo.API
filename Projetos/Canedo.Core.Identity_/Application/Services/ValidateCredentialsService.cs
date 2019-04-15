using Canedo.Core.Identity.Application.Interfaces;
using Canedo.Core.Identity.Domain.Interfaces;

namespace Canedo.Core.Identity.Application.Services
{
    public class ValidateCredentialsService : IValidateCredentialsService
    {
        readonly IValidateUserService validateUserService_;

        public ValidateCredentialsService(IValidateUserService validateUserService)
        {   
            validateUserService_ = validateUserService;
        }
        
        #region ..:: Publics ::..

        public bool ValidateUser(string userId, string password)
        {
            return validateUserService_.ValidateUser(userId, password);
        }

        public bool ValidateRole(string role)
        {
            return validateUserService_.ValidateRole(role);
        }

        #endregion
    }
}
