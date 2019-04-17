using Canedo.Core.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;

namespace Canedo.Core.Identity.Infra.Configuration
{
    public class IdentityConfiguration<T> 
        where T : IdentityUser
    {
        private readonly CustomIdentityDbContext<T> _context;
        private readonly UserManager<T> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const string _role = "AdminRoot";


        public IdentityConfiguration(
            CustomIdentityDbContext<T> context,
            UserManager<T> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_context.Database.EnsureCreated())
            {
                if (!_roleManager.RoleExistsAsync(_role).Result)
                {
                    var resultado = _roleManager.CreateAsync(
                        new IdentityRole(_role)).Result;
                    if (!resultado.Succeeded)
                    {
                        throw new Exception(
                            $"Erro durante a criação da role {_role}.");
                    }
                }

                CreateUser(ResolveInstance("admin", "admin@canedo.com.br", true), "admin@123mudar", _role);
            }
        }

        private void CreateUser(
            T user,
            string password,
            string initialRole = null)
        {
            if (_userManager.FindByNameAsync(user.UserName).Result == null)
            {
                var resultado = _userManager
                    .CreateAsync(user, password).Result;

                if (resultado.Succeeded &&
                    !String.IsNullOrWhiteSpace(initialRole))
                {
                    _userManager.AddToRoleAsync(user, initialRole).Wait();
                }
            }
        }

        private T ResolveInstance(string userName, string email, bool emailConfirmed)
        {
            var instance = Activator.CreateInstance<T>();

            instance.UserName = userName;
            instance.Email = email;
            instance.EmailConfirmed = emailConfirmed;

            return instance;
        }
    }
}
