using Canedo.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canedo.Domain.Core.Users.Interfaces.Services
{
    public interface IUserService
    {
        User FindUser(string userId); 
    }
}
