using System.Threading.Tasks;

namespace Umbraco.Docs.Preview.App.Services
{
    public interface IDocumentationChangeNotifier
    {
        Task<bool> WaitForChanges();
        void PublishChangeNotifications();
    }
}
