using System.Collections.Generic;
using Umbraco.Docs.Preview.UI.Interceptors.Caching;
using Umbraco.Docs.Preview.UI.Models;

namespace Umbraco.Docs.Preview.UI.Services
{
    public interface IDocumentService
    {
        bool TryFindMarkdownFile(string slug, out DocumentVersion version);
        IEnumerable<DocumentVersion> GetAlternates(DocumentVersion version);

        [CacheIndefinitely(CacheKey = nameof(GetDocsTree))]
        public UmbracoDocsTreeNode GetDocsTree();
    }
}
