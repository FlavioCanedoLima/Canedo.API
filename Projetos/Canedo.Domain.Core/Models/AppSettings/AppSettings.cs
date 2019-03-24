namespace Canedo.Domain.Core.Models.AppSettings
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public TokenConfigurations TokenConfigurations { get; set; }
        public Ravendb Ravendb { get; set; }
    }
}
