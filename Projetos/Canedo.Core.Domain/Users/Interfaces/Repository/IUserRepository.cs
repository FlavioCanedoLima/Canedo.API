using Canedo.Core.Domain.Models;

namespace Canedo.Core.Domain.Users.Interfaces.Repository
{
    public interface IUserRepository
    {
        User GetUser(string userId);
    }
}
