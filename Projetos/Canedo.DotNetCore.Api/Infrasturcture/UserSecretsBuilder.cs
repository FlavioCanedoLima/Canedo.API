using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Canedo.DotNetCore.Api
{
    public class UserSecretsBuilder
    {
        private IConfiguration _configuration;
        readonly IHostingEnvironment _environment;

        public UserSecretsBuilder(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void Build()
        {
            XmlDocument xmldoc = new XmlDocument();

            xmldoc.Load($@"{((PhysicalFileProvider)_environment.ContentRootFileProvider).Root}{_environment.ApplicationName}.csproj");

            var userSecretId = xmldoc.SelectSingleNode("//UserSecretsId").InnerText;

            if (userSecretId.Any() == false) return; 

            var builder =
                new ConfigurationBuilder()
                .SetBasePath(_environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = 
                builder
                .AddUserSecrets(userSecretId)
                .Build();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await context.Response.WriteAsync("------- Before ------ \n\r");

            //wait _next(context);

            await context.Response.WriteAsync("\n\r------- After ------");
        }
    }
}
