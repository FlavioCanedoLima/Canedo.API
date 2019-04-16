using Canedo.Core.Identity.Domain.Interfaces;
using Canedo.Core.Identity.Domain.UserApi.Model;
using Microsoft.AspNetCore.Identity;

namespace Canedo.Core.Identity.Domain.UserApi.Services
{
    public class ValidateUserApiService : IValidateUserService
    {
        readonly UserManager<IdentityUserApi> userManager_;
        readonly SignInManager<IdentityUserApi> signInManager_;

        public ValidateUserApiService(
            UserManager<IdentityUserApi> userManager, 
            SignInManager<IdentityUserApi> signInManager)
        {
            userManager_ = userManager;
            signInManager_ = signInManager;
        }

        public IdentityUserApi CurrentIdentityUser { get; set; }

        public bool ValidateUser(string userId, string password)
        {
            CurrentIdentityUser = GetIdentityUser(userId);

            if (CurrentIdentityUser is null) return false;

            return ValidatePassword(CurrentIdentityUser, password).Succeeded;
        }

        public bool ValidateRole(string role)
        {
            if (CurrentIdentityUser is null) return false;

            return userManager_.IsInRoleAsync(CurrentIdentityUser, role).Result;
        }

        private IdentityUserApi GetIdentityUser(string userId)
        {
            return
                userManager_
                .FindByNameAsync(userId)
                .Result;
        }

        private SignInResult ValidatePassword(IdentityUserApi userIdentity, string password)
        {
            return
                signInManager_
                .CheckPasswordSignInAsync(userIdentity, password, false)
                .Result;
        }
    }
}
