using Canedo.Core.Infra.JWT.Configuration;
using Canedo.DotNetCore.Api.Infra.AppSettings;

namespace Canedo.DotNetCore.Api.Infra
{
    public class SigningConfiguration : SigningConfigurationBase
    {
        public SigningConfiguration(TokenConfiguration tokenConfiguration)
        {
            TokenConfiguration = tokenConfiguration;

            Configurations();
        }

        public override void Configurations()
        {
            base.Configurations();
        }
    }
}
