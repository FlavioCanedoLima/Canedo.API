using Canedo.Core.Infra.JWT.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Canedo.Core.Infra.JWT.Interfaces
{
    public interface ISigningConfiguration
    {
        SecurityKey Key { get; }
        SigningCredentials SigningCredentials { get; }
        TokenConfiguration TokenConfiguration { get; }
        void Configurations();
    }
}
