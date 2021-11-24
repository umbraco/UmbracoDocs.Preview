using Castle.DynamicProxy;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Umbraco.Docs.Preview.UI.Extensions;
using Umbraco.Docs.Preview.UI.Interceptors;
using Umbraco.Docs.Preview.UI.Interceptors.Caching;
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

        // ReSharper disable once UnusedMember.Global -- Used by Lamar
        public void ConfigureContainer(ServiceRegistry services)
        {
            services.AddMvc();
            services.AddHttpContextAccessor();
            
            services.AddSingleton<IMarkdownService, MarkdownService>();
            services.AddSingleton<DocumentationUpdater>();
            services.AddTransient<IConfigureOptions<UmbracoDocsOptions>, ConfigureUmbracoDocsOptions>();

            services.For<IDocumentService>()
                .Add<DocumentService>()
                .OnCreation((container, svc) => new ProxyGenerator().CreateInterfaceProxyWithTarget((IDocumentService)svc, container.GetInstance<CachingInterceptor>()));
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
