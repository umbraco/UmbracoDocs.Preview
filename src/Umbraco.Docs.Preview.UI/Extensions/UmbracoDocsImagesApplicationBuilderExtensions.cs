using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Umbraco.Docs.Preview.UI.MiscellaneousOurStuff;
using Umbraco.Docs.Preview.UI.Models;

namespace Umbraco.Docs.Preview.UI.Extensions
{
    public static class UmbracoDocsImagesApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseUmbracoDocsImageFileProviders(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;

            var tree = serviceProvider.GetRequiredService<DocumentationUpdater>().BuildSitemap();

            var log = serviceProvider
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger(typeof(UmbracoDocsImagesApplicationBuilderExtensions));

            log.LogWarning("Images from new documentation sub directories will not be served without a restart");

            AddImageFileProviders(tree, app, log);

            return app;
        }

        private static void AddImageFileProviders(UmbracoDocsTreeNode node, IApplicationBuilder app, ILogger log)
        {
            var path = Path.Combine(node.PhysicalPath, "images");
            var requestPath = $"/documentation/{node.Path}/images".Replace("//", "/"); // Hack for root UmbracoDocs folder

            if (Directory.Exists(path))
            {
                log.LogDebug("Adding file provider for {path}", path);

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(path),
                    RequestPath = requestPath
                });
            }

            foreach (var child in node.Directories)
            {
                AddImageFileProviders(child, app, log);
            }
        }
    }
}
