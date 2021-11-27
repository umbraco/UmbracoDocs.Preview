using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Umbraco.Docs.Preview.UI.Services;

namespace Umbraco.Docs.Preview.UI.Messaging.Notifications.DocumentationUpdated
{
    public class TriggerBrowserReload : INotificationHandler<DocsUpdated>
    {
        private readonly ILogger _log;
        private readonly IDocumentationChangeNotifier _notifier;

        public TriggerBrowserReload(
            ILogger<TriggerBrowserReload> log, 
            IDocumentationChangeNotifier notifier)
        {
            _log = log;
            _notifier = notifier;
        }

        public Task Handle(
            DocsUpdated notification, 
            CancellationToken cancellationToken)
        {
            _log.LogInformation("Documentation updated, triggering UI reload.");
            _notifier.PublishChangeNotifications();
            return Task.CompletedTask;
        }
    }
}
