using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Umbraco.Docs.Preview.App.MiscellaneousOurStuff;
using Umbraco.Docs.Preview.App.Models;

namespace Umbraco.Docs.Preview.App.Extensions
{
    public static class UmbracoDocsApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseUmbracoDocsImageFileProviders(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;

            var tree = serviceProvider.GetRequiredService<DocumentationUpdater>().BuildSitemap();

            var log = serviceProvider
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger(typeof(UmbracoDocsApplicationBuilderExtensions));

            var total = AddImageFileProviders(tree, app, log);
            log.LogInformation("Found {count} image directories for UmbracoDocs", total);

            log.LogWarning("Images from new documentation sub directories will not be served without a restart");

            return app;
        }

        private static int AddImageFileProviders(UmbracoDocsTreeNode node, IApplicationBuilder app, ILogger log)
        {
            var counter = 0;
            var path = Path.Combine(node.PhysicalPath, "images");

            var requestPath = $"/documentation/{node.Path}/images"
                .Replace("//", "/"); // Hack for root UmbracoDocs folder

            if (Directory.Exists(path))
            {
                log.LogDebug("Adding file provider for {path}", path);

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(path),
                    RequestPath = requestPath
                });

                ++counter;
            }

            // Could add these to a composite?
            foreach (var child in node.Directories)
            {
                counter += AddImageFileProviders(child, app, log);
            }

            return counter;
        }
    }
}
