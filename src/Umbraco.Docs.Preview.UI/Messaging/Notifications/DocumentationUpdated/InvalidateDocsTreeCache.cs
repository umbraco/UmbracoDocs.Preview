using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Umbraco.Docs.Preview.UI.Services;

namespace Umbraco.Docs.Preview.UI.Messaging.Notifications.DocumentationUpdated
{
    public class InvalidateDocsTreeCache : INotificationHandler<DocsUpdated>
    {
        private readonly ILogger _log;
        private readonly IMemoryCache _memoryCache;

        public InvalidateDocsTreeCache(
            ILogger<InvalidateDocsTreeCache> log,
            IMemoryCache memoryCache)
        {
            _log = log;
            _memoryCache = memoryCache;
        }

        public Task Handle(DocsUpdated notification, CancellationToken cancellationToken)
        {
            _log.LogDebug("Clearing DocsTree cache.");
            _memoryCache.Remove(nameof(IDocumentService.GetDocsTree));
            return Task.CompletedTask;
        }
    }
}
