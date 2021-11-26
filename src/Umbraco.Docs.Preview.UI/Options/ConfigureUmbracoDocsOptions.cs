using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Umbraco.Docs.Preview.UI.Options
{
    public class ConfigureUmbracoDocsOptions : IConfigureOptions<UmbracoDocsOptions>
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ConfigureUmbracoDocsOptions> _log;

        public ConfigureUmbracoDocsOptions(
            IConfiguration configuration,
            IWebHostEnvironment env,
            ILogger<ConfigureUmbracoDocsOptions> log)
        {
            _configuration = configuration;
            _env = env;
            _log = log;
        }

        public void Configure(UmbracoDocsOptions options)
        {
            var directory = Directory.GetCurrentDirectory();
            
            if (!File.Exists(Path.Combine(directory, "index.md")))
            {
                _log.LogCritical("{dir} directory doesn't appear to be the UmbracoDocs repo.", directory);
                return;
            }

            options.UmbracoDocsRootFolder = directory;
        }
    }
}
