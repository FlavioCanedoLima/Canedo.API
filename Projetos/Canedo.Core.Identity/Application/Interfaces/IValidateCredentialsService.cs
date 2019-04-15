using Microsoft.AspNetCore.Identity;

namespace Canedo.Core.Identity.Application.Interfaces
{
    public interface IValidateCredentialsService
    {
        bool ValidateUser(string userId, string password);
        bool ValidateRole(string role);
    }
}
