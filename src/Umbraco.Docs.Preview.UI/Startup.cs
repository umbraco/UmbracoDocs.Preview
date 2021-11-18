using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Umbraco.Docs.Preview.UI.Extensions;
using Umbraco.Docs.Preview.UI.MiscellaneousOurStuff;
using Umbraco.Docs.Preview.UI.Options;
using Umbraco.Docs.Preview.UI.Services;

namespace Umbraco.Docs.Preview.UI
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddHttpContextAccessor();
            services.AddSingleton<IDocumentService, DocumentService>();
            services.AddSingleton<IMarkdownService, MarkdownService>();
            services.AddSingleton<DocumentationUpdater>();
            services.AddTransient<IConfigureOptions<UmbracoDocsOptions>, ConfigureUmbracoDocsOptions>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStatusCodePages();

            app.UseUmbracoDocsImageFileProviders();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
