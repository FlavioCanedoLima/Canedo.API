﻿namespace Canedo.DotNetCore.Api.Infra.AppSettings
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
