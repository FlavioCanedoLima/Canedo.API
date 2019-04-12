﻿using Canedo.Domain.Core.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Canedo.Domain.Core.Models
{
    public abstract class SigningConfigurationBase : ISigningConfiguration
    {
        public SecurityKey Key { get; set; }
        public SigningCredentials SigningCredentials { get; set; }

        public virtual void Configurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
                Key = new RsaSecurityKey(provider.ExportParameters(true));

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
