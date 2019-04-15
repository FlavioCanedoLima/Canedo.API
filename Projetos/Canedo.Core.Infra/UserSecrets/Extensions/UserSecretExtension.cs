using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Linq;
using System.Xml;

namespace Canedo.Core.Infra.UserSecrets.Extensions
{
    public static class UserSecretExtension
    {
        public static IHostingEnvironment UseUserSecrets(this IHostingEnvironment environment)
        {
            XmlDocument xmldoc = new XmlDocument();

            xmldoc.Load($@"{((PhysicalFileProvider)environment.ContentRootFileProvider).Root}{environment.ApplicationName}.csproj");

            var userSecretId = xmldoc.SelectSingleNode("//UserSecretsId").InnerText;

            if (userSecretId.Any() == false) return environment;

            new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets(userSecretId)
                .Build();

            return environment;
        }
    }
}
