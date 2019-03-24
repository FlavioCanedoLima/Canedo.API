using Canedo.Domain.Core.Models;
using Canedo.Domain.Core.Users.Interfaces.Repository;
using Canedo.DotNetCore.Data.Configuration;


namespace Canedo.DotNetCore.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        readonly private RavenConfig _ravenConfig;

        public UserRepository(RavenConfig ravenConfig)
        {
            _ravenConfig = ravenConfig;
        }

        public User GetUser(string userId)
        {
            using (var session = _ravenConfig.OpenSession())
            {
                return
                    session
                    .Advanced
                    .DocumentQuery<User>()
                    .WhereEquals(w => w.UserId, userId)
                    .FirstOrDefault();
            }
        }

        public string SaveUser(User user)
        {
            using (var session = _ravenConfig.OpenSession())
            {
                session.Store(user);
                session.SaveChanges();
                return user.Id;
            }
        }
    }
}
