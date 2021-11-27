using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Lamar;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Umbraco.Docs.Preview.UI.Extensions;
using Umbraco.Docs.Preview.UI.HostedServices;
using Umbraco.Docs.Preview.UI.Interceptors.Caching;
using Umbraco.Docs.Preview.UI.MiscellaneousOurStuff;
using Umbraco.Docs.Preview.UI.Options;
using Umbraco.Docs.Preview.UI.Services;

namespace Umbraco.Docs.Preview.UI
{
    public class Startup
    {
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
                .OnCreation((sp, svc) =>
                {
                    var target = (IDocumentService) svc;
                    var caching = sp.GetInstance<CachingInterceptor>();
                    
                    return new ProxyGenerator()
                        .CreateInterfaceProxyWithTarget(target, caching);
                });

            services.AddHostedService<DocumentationFolderMonitor>();

            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var opts = app.ApplicationServices.GetRequiredService<IOptions<UmbracoDocsOptions>>();
            if (opts.Value.UmbracoDocsRootFolder == null)
            {
                Task.Run(() =>
                {
                    // TODO: Find better graceful shutdown.
                    Thread.Sleep(1000);
                    app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>().StopApplication();
                });

                return;
            }

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
            app.UseOurUmbracoEmbeddedResources();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
