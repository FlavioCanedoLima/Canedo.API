using Canedo.Core.Identity.Domain.UserApi.Model;

namespace Canedo.Core.Identity.Domain.Interfaces
{
    public interface IValidateUserService
    {
        IdentityUserApi CurrentIdentityUser { get; }
        bool ValidateUser(string userId, string password);
        bool ValidateRole(string role);
    }
}
