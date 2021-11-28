using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Umbraco.Docs.Preview.App.Services;

namespace Umbraco.Docs.Preview.App.Messaging.Notifications.DocumentationUpdated
{
    public class DocsUpdatedHandler : INotificationHandler<DocsUpdated>
    {
        private readonly ILogger _log;
        private readonly IMemoryCache _memoryCache;
        private readonly IDocumentationChangeNotifier _clientNotifier;

        public DocsUpdatedHandler(
            ILogger<DocsUpdatedHandler> log,
            IMemoryCache memoryCache,
            IDocumentationChangeNotifier clientNotifier)
        {
            _log = log;
            _memoryCache = memoryCache;
            _clientNotifier = clientNotifier;
        }

        public Task Handle(DocsUpdated notification, CancellationToken cancellationToken)
        {
            _log.LogDebug("Documentation updated clearing DocsTree cache.");
            _memoryCache.Remove(nameof(IDocumentService.GetDocsTree));

            _log.LogInformation("Documentation updated, triggering UI reload.");
            _clientNotifier.PublishChangeNotifications();
         
            return Task.CompletedTask;
        }
    }
}
