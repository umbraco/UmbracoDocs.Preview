using System;
using System.IO;
using System.Reflection;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Umbraco.Docs.Preview.App
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var directory = Directory.GetCurrentDirectory();
            if (!File.Exists(Path.Combine(directory, "index.md")))
            {
                Console.Error.WriteLine("Fatal Error: current directory doesn't appear to be the UmbracoDocs repo.");
                return -1;
            }

            CreateHostBuilder(args)
                .Build()
                .Run();

            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // dotnet tools configure cwd as you would expect, but Host.CreateDefaultBuilder isn't expecting that 
            // to be miles away from appsettings.json and wwwroot etc.
            // So more config, less convention for ContentRoot
            var applicationFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return Host.CreateDefaultBuilder(args)
                .UseContentRoot(applicationFolder)
                .UseLamar()
                .UseConsoleLifetime()
                .ConfigureWebHostDefaults(webBuilder =>
                {
#if !DEBUG
                    webBuilder.UseEnvironment("Production");
#endif
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
