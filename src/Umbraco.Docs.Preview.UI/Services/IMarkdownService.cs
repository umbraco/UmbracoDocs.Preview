using Umbraco.Docs.Preview.UI.Models;

namespace Umbraco.Docs.Preview.UI.Services
{
    public interface IMarkdownService
    {
        string RenderMarkdown(DocumentVersion version);
    }
}
