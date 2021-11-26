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
                    cfg.SetMinimumLevel(LogLevel.Warning);
                    cfg.AddFilter((_, log, level) => log.StartsWith("Umbraco") && level >= LogLevel.Debug);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
