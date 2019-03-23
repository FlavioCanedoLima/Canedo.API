using Canedo.Domain.Core.Users.Interfaces.Repository;
using System.Security.Cryptography.X509Certificates;

namespace Canedo.DotNetCore.Data.Configuration
{
    public class AdminCertificate : IRavenCertificateRepository
    {
        public X509Certificate2 Certificate => new X509Certificate2(@"Certificate\admin.client.certificate.canedo.pfx");
    }
}
