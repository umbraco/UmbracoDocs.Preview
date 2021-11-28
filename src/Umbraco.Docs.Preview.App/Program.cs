using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Reflection;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Umbraco.Docs.Preview.App.Options;

namespace Umbraco.Docs.Preview.App
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Argument
                {
                    Name = "path",
                    Arity = new ArgumentArity(0, 1),
                    Description = "Path to UmbracoDocs repository."
                }
            };
            rootCommand.Name = "umbracodocs";
            rootCommand.Description = "Run UmbracoDocs preview server";
            rootCommand.Handler = CommandHandler.Create<string>(RunServer);

            return rootCommand.Invoke(args);
        }

        private static int RunServer(string path = null)
        {
            path ??= Directory.GetCurrentDirectory();

            var docsAbsolutePath = Path.GetFullPath(path);


            if (!File.Exists(Path.Combine(docsAbsolutePath!, "index.md")))
            {
                Console.Error.WriteLine("Fatal Error: current directory doesn't appear to be the UmbracoDocs repo.");
                return -1;
            }

            Host.CreateDefaultBuilder(new[] { path })
#if !DEBUG
                // dotnet tools configures cwd as you would expect, but Host.CreateDefaultBuilder isn't expecting that 
                // to be miles away from appsettings.json and wwwroot etc.
                // So more prefer configuration over convention for ContentRoot (unless debugging).
                .UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
#endif 
                .UseLamar()
                .UseConsoleLifetime()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services =>
                    {
                        services.Configure<UmbracoDocsOptions>(cfg => cfg.UmbracoDocsRootFolder = docsAbsolutePath);
                    });
                    webBuilder.UseStartup<Startup>();

                })
                .Build()
                .Run();

            return 0;
        }
    }
}
