using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Umbraco.Docs.Preview.UI.Services
{
    internal class DocumentationChangeNotifier : IDocumentationChangeNotifier
    {
        private static readonly ConcurrentQueue<TaskCompletionSource<bool>> Subscribers = new();

        public async Task<bool> WaitForChanges()
        {
            var completionSource = new TaskCompletionSource<bool>();

            Subscribers.Enqueue(completionSource);

            var task = await Task.WhenAny(completionSource.Task, Task.Delay(30 * 1000));

            if (task is Task<bool> completionResult)
            {
                return completionResult.Result;
            }

            return false;
        }

        public void PublishChangeNotifications()
        {
            while (Subscribers.TryDequeue(out var subscriber))
            {
                subscriber.SetResult(true);
            }
        }
    }
}
