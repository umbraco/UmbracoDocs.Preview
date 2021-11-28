using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
// ReSharper disable once RedundantUsingDirective
using System.Reflection; // Used in Release build.
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
                    Description = "Optional path to UmbracoDocs repository. Defaults to current directory when omitted."
                }
            };
            rootCommand.Name = "umbracodocs";
            rootCommand.Description = "Run UmbracoDocs preview server";
            rootCommand.Handler = CommandHandler.Create<string>(RunServer);

            var result = rootCommand.Invoke(args);

            switch (result)
            {
                case -1:
                    rootCommand.Invoke("--help");
                    break;
            }

            return result;
        }

        private static int RunServer(string path = null)
        {
            path ??= Directory.GetCurrentDirectory();

            var docsAbsolutePath = Path.GetFullPath(path);

            if (!File.Exists(Path.Combine(docsAbsolutePath, "index.md")))
            {
                Console.Error.WriteLine($"Specified path doesn't appear to be the UmbracoDocs repository.{Environment.NewLine}");
                return -1;
            }

            Host.CreateDefaultBuilder(new[] { path })
#if !DEBUG
                // Host.CreateDefaultBuilder sets ContentRoot to cwd which will cause issues
                // when this app is run from its installed location i.e. ~/.dotnet/tools
                // e.g. wwwroot & appsettings.json won't be found.
                // Prefer configuration over convention for ContentRoot (except when debugging).
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
