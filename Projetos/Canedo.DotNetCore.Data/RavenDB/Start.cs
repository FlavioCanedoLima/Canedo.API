using Canedo.DotNetCore.Data.Repository;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Canedo.DotNetCore.Data.RavenDB
{
    public class Start
    {

        public Start()
        {
            //X509Certificate2 clientCertificate = new X509Certificate2(@"Certificate\admin.client.certificate.canedo.pfx");

            //using (IDocumentStore store = 
            //    new DocumentStore()
            //    {
            //        Certificate = clientCertificate,
            //        Database = "Canedo.Api",
            //        Urls = new[] { "https://home.canedo.development" }
            //    }
            //    .Initialize())
            //    {
            //        // do your work here
            //    }
        }

        public void GetRepository()
        {
            var t = new UserRepository().GetUser("userId");
        }
    }
}