using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Umbraco.Docs.Preview.UI.Options
{
    public class ConfigureUmbracoDocsOptions : IConfigureOptions<UmbracoDocsOptions>
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public ConfigureUmbracoDocsOptions(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void Configure(UmbracoDocsOptions options)
        {
            var configuredPath = _configuration.GetValue<string>("UmbracoDocs:RepositoryDirectory");

            options.UmbracoDocsRootFolder = string.IsNullOrEmpty(configuredPath)
                ? FindUmbracoDocsSubmoduleDirectory()
                : configuredPath;
        }

        private string FindUmbracoDocsSubmoduleDirectory()
        {
            var directory = new DirectoryInfo(_env.WebRootPath);

            while (directory != null)
            {
                var testPath = Path.Combine(directory.ToString(), "submodules", "UmbracoDocs");

                if (Directory.Exists(testPath))
                {
                    return testPath;
                }

                directory = directory.Parent;
            }

            throw new ApplicationException("Couldn't find UmbracoDocs directory, did you forget to $ git submodule init/update ?");
        }
    }
}
