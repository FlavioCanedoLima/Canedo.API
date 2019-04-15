namespace Canedo.DotNetCore.Api.Infra.AppSettings
{
    public class AppSetting
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public TokenConfigurations TokenConfigurations { get; set; }
        public Ravendb Ravendb { get; set; }
    }
}
