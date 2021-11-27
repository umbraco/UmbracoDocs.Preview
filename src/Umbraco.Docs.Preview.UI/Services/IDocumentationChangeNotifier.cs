using System.Threading.Tasks;

namespace Umbraco.Docs.Preview.UI.Services
{
    public interface IDocumentationChangeNotifier
    {
        Task<bool> WaitForChanges();
        void PublishChangeNotifications();
    }
}
