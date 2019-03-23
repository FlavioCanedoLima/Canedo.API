using System;
using System.Security.Cryptography.X509Certificates;

namespace Canedo.Domain.Core.Users.Interfaces.Repository
{
    public interface IRavenCertificateRepository
    {
        X509Certificate2 Certificate { get; }
    }
}
