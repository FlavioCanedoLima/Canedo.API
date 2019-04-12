using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Canedo.Domain.Application.User.Data
{
    public class CustomIdentityDbContext<T> 
        : IdentityDbContext<T>
        where T : IdentityUser
    {
        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext<T>> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
