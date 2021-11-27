using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Umbraco.Docs.Preview.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseLamar()
                .UseConsoleLifetime()
                .ConfigureLogging(cfg =>
                {
                    cfg.AddConsole();
                    cfg.AddDebug();
                    cfg.AddFilter((_, log, level) =>
                    {
                        // TODO: move to config somewhere.
                        if (log.StartsWith("Umbraco") && level >= LogLevel.Debug)
                            return true;
                        if (log.StartsWith("Microsoft.Hosting.Lifetime") && level >= LogLevel.Debug)
                            return true;
                        return false;
                    });
                    cfg.SetMinimumLevel(LogLevel.Warning);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
