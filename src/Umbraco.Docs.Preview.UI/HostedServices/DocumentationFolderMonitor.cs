using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Docs.Preview.UI.Options;

namespace Umbraco.Docs.Preview.UI.HostedServices
{
    public class DocumentationFolderMonitor : IHostedService
    {
        private readonly ILogger<DocumentationFolderMonitor> _log;
        private readonly IOptions<UmbracoDocsOptions> _options;

        private FileSystemWatcher _watcher;

        public DocumentationFolderMonitor(
            ILogger<DocumentationFolderMonitor> log,
            IOptions<UmbracoDocsOptions> options)
        {
            _log = log;
            _options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            SetupWatcher();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            TearDownWatcher();
            return Task.CompletedTask;
        }

        private void OnChange(object sender, FileSystemEventArgs args)
        {
            _log.LogInformation("Documentation changed, reloading UI", args.Name);
        }

        private void OnError(object sender, ErrorEventArgs args)
        {
            TearDownWatcher();
            SetupWatcher();
        }

        private void SetupWatcher()
        {
            _watcher = new FileSystemWatcher(_options.Value.UmbracoDocsRootFolder, "*.md");
            _watcher.IncludeSubdirectories = true;
            _watcher.Changed += OnChange;
            _watcher.Created += OnChange;
            _watcher.Deleted += OnChange;
            _watcher.Renamed += OnChange;
            _watcher.Error += OnError;
            _watcher.EnableRaisingEvents = true;
        }

        private void TearDownWatcher()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Changed -= OnChange;
            _watcher.Created -= OnChange;
            _watcher.Deleted -= OnChange;
            _watcher.Renamed -= OnChange;
            _watcher.Error -= OnError;
            _watcher.Dispose();
        }
    }
}
