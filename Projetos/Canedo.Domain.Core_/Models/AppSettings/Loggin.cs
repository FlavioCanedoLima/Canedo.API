namespace Canedo.Domain.Core.Models.AppSettings
{
    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
    }
}
