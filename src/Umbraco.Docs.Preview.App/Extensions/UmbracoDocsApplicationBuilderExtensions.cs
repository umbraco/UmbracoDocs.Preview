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

            AddImageFileProviders(tree, app, log);

            log.LogWarning("Images from new documentation sub directories will not be served without a restart");

            return app;
        }

        private static void AddImageFileProviders(UmbracoDocsTreeNode node, IApplicationBuilder app, ILogger log)
        {
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
            }

            foreach (var child in node.Directories)
            {
                AddImageFileProviders(child, app, log);
            }
        }

        public static IApplicationBuilder UseOurUmbracoEmbeddedResources(this IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new ManifestEmbeddedFileProvider(typeof(Program).Assembly, "wwwroot"),
                RequestPath = ""
            });
            return app;
        }
    }
}
