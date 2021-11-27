using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Umbraco.Docs.Preview.UI.Messaging.Notifications.DocumentationUpdated
{
    public class TriggerBrowserReload : INotificationHandler<DocsUpdated>
    {
        private readonly ILogger _log;

        public TriggerBrowserReload(ILogger<TriggerBrowserReload> log)
        {
            _log = log;
        }

        public Task Handle(DocsUpdated notification, CancellationToken cancellationToken)
        {
            // TODO: actually reload UI.
            _log.LogInformation("Documentation updated, triggering UI reload (TODO:).");
            return Task.CompletedTask;
        }
    }
}
