using Canedo.Domain.Core.Models.AppSettings;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Canedo.DotNetCore.Data.Configuration
{
    public class RavenConfig : IDisposable
    {   
        readonly private Session _session;

        public RavenConfig(AppSettings appSettings)
        {
            _session = appSettings.Ravendb.Sessions.Where(w => w.DataBaseName.Equals("Canedo.Api")).FirstOrDefault();
        }

        private IDocumentSession DocumentSession { get; set; }

        public IDocumentSession OpenSession()
        {
            DocumentSession =
                new DocumentStore()
                {
                    Certificate = new X509Certificate2(_session.CertificateName),//_x509Certificate2.Certificate,
                    Database = _session.DataBaseName,
                    Urls = _session.Urls
                }
                .Initialize()
                .OpenSession();

            return DocumentSession;
        }

        public void Dispose()
        {
            if (DocumentSession is null) return;

            DocumentSession.Dispose();
        }
    }
}
