using System.Collections.Generic;
using Umbraco.Docs.Preview.UI.Models;

namespace Umbraco.Docs.Preview.UI.Services
{
    public interface IDocumentService
    {
        bool TryFindMarkdownFile(string slug, out DocumentVersion version);
        IEnumerable<DocumentVersion> GetAlternates(DocumentVersion version);
        public UmbracoDocsTreeNode GetDocsTree();
    }
}
