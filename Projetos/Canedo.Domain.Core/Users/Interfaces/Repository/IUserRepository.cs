using Canedo.Domain.Core.Models;

namespace Canedo.Domain.Core.Users.Interfaces.Repository
{
    public interface IUserRepository
    {
        User GetUser(string userId);
    }
}
