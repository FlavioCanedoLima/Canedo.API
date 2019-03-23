using Canedo.Domain.Core.Models;
using Canedo.Domain.Core.Users.Interfaces.Repository;
using Canedo.DotNetCore.Data.Configuration;


namespace Canedo.DotNetCore.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        public User GetUser(string userId)
        {
            using (var session = new RavenConfig<AdminCertificate, HomeNode>().InitDocumentStore("Canedo.Api"))
            {
                
                return new User();
            }
        }
    }
}
