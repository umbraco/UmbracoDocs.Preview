using Umbraco.Docs.Preview.App.Models;

namespace Umbraco.Docs.Preview.App.Services
{
    public interface IMarkdownService
    {
        string RenderMarkdown(DocumentVersion version);
    }
}
