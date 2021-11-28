using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Umbraco.Docs.Preview.App.Options;

namespace Umbraco.Docs.Preview.App.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseUmbracoDocsStaticFiles(this IApplicationBuilder app)
        {
            var settings = app.ApplicationServices.GetRequiredService<IOptions<UmbracoDocsOptions>>();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(settings.Value.UmbracoDocsRootFolder),
                RequestPath = "/documentation"
            });

            return app;
        }
    }
}
