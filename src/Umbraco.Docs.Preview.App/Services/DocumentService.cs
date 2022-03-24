using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using Umbraco.Docs.Preview.App.MiscellaneousOurStuff;
using Umbraco.Docs.Preview.App.Models;
using Umbraco.Docs.Preview.App.Options;

namespace Umbraco.Docs.Preview.App.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IOptions<UmbracoDocsOptions> _umbracoDocsOptions;
        private readonly DocumentationUpdater _ourDocsUpdater;

        public DocumentService(IOptions<UmbracoDocsOptions> umbracoDocsOptions, DocumentationUpdater ourDocsUpdater)
        {
            _umbracoDocsOptions = umbracoDocsOptions ?? throw new ArgumentNullException(nameof(umbracoDocsOptions));
            _ourDocsUpdater = ourDocsUpdater ?? throw new ArgumentNullException(nameof(ourDocsUpdater));
        }

        public string DocsRoot => _umbracoDocsOptions.Value.UmbracoDocsRootFolder;

        public bool TryFindMarkdownFile(string slug, out DocumentVersion version)
        {
            if (slug == null)
            {
                version = new DocumentVersion(DocsRoot, "index.md");
                return File.Exists(version.FileSystemPath);
            }

            slug = slug.UnderscoreToDot();

            var slugParts = slug.Split("/")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();


            version = new DocumentVersion(DocsRoot, $"{slugParts.Last()}.md", slugParts.SkipLast(1).ToArray());
            if (File.Exists(version.FileSystemPath))
            {
                return true;
            }

            version = new DocumentVersion(DocsRoot, "index.md", slugParts);
            if (File.Exists(version.FileSystemPath))
            {
                return true;
            }

            version = null;
            return false;
        }

        public IEnumerable<DocumentVersion> GetAlternates(DocumentVersion version)
        {
            // TODO: parse version numbers, might not even be worth it.

            return Directory
                .GetFiles(Path.GetDirectoryName(version.FileSystemPath)!)
                .Where(x =>
                {
                    var unVersioned = version.FileNameWithoutExtension.Split("-v").First();
                    var alt = Path.GetFileNameWithoutExtension(x);

                    return alt.StartsWith(unVersioned);
                })
                .Select(x => new DocumentVersion(DocsRoot, Path.GetFileName(x), version.RelativePathSegments))
                .OrderByDescending(x => x.FileName);
        }

        public UmbracoDocsTreeNode GetDocsTree()
        {
            return _ourDocsUpdater.BuildSitemap();
        }
    }
}
