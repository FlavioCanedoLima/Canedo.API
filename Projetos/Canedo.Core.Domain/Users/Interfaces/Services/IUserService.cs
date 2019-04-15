using Canedo.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canedo.Core.Domain.Users.Interfaces.Services
{
    public interface IUserService
    {
        User FindUser(string userId); 
    }
}
