using System.Collections.Generic;
using Umbraco.Docs.Preview.UI.Models;

namespace Umbraco.Docs.Preview.UI.Services
{
    public interface IDocumentService
    {
        bool TryFindMarkdownFile(string slug, out DocumentationVersion version);
        IEnumerable<DocumentationVersion> GetAlternates(DocumentationVersion version);
        public UmbracoDocsTreeNode GetDocsTree();
    }
}
