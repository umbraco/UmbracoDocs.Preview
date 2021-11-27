using System.Collections.Generic;
using Umbraco.Docs.Preview.App.Interceptors.Caching;
using Umbraco.Docs.Preview.App.Models;

namespace Umbraco.Docs.Preview.App.Services
{
    public interface IDocumentService
    {
        bool TryFindMarkdownFile(string slug, out DocumentVersion version);
        IEnumerable<DocumentVersion> GetAlternates(DocumentVersion version);

        [CacheIndefinitely(CacheKey = nameof(GetDocsTree))]
        public UmbracoDocsTreeNode GetDocsTree();
    }
}
