namespace Canedo.Domain.Core.Models.AppSettings
{
    public class Ravendb
    {
        public Session[] Sessions { get; set; }
    }

    public class Session
    {
        public string[] Urls { get; set; }
        public string DataBaseName { get; set; }
        public string CertificateName { get; set; }
    }
}
