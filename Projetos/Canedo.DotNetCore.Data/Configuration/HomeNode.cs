using Canedo.Domain.Core.Users.Interfaces.Repository;
using System;

namespace Canedo.DotNetCore.Data.Configuration
{
    public class HomeNode : IRavenNodeRespository
    {
        public string[] Urls => new string[] { "https://home.canedo.development" };
    }
}
