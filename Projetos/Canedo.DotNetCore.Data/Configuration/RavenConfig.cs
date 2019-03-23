using Canedo.Domain.Core.Users.Interfaces.Repository;
using Raven.Client.Documents;
using System;

namespace Canedo.DotNetCore.Data.Configuration
{
    public class RavenConfig<C, N> : IDisposable
        where C : IRavenCertificateRepository  
        where N : IRavenNodeRespository
    {
        readonly private IRavenCertificateRepository _x509Certificate2;
        readonly private IRavenNodeRespository _ravenNodeRespository;

        public RavenConfig()
        {
            _x509Certificate2 = Activator.CreateInstance<C>();
            _ravenNodeRespository = Activator.CreateInstance<N>();
        }

        public IDocumentStore DocumentStore { get; private set; }

        public IDocumentStore InitDocumentStore(string database)
        {
            DocumentStore = 
                new DocumentStore()
                {
                    Certificate = _x509Certificate2.Certificate,//new X509Certificate2(@"Certificate\admin.client.certificate.canedo.pfx"),
                    Database = database,
                    Urls = _ravenNodeRespository.Urls//new[] { "https://home.canedo.development" }
                }
                .Initialize();

            return DocumentStore;
        }

        public void Dispose()
        {
            DocumentStore.Dispose();
        }
    }
}
