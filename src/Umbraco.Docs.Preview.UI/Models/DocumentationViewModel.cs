using System.Collections.Generic;

namespace Umbraco.Docs.Preview.UI.Models
{
    public class DocumentationViewModel
    {
        public DocumentVersion DocumentVersion { get; init; }
        public string Markup { get; init; }
        public UmbracoDocsTreeNode Navigation { get; init; }
        public List<DocumentVersion> Alternates { get; init; }

        public bool IsCurrent(DocumentVersion alternate)
        {
            return DocumentVersion.FileSystemPath.Equals(alternate.FileSystemPath);
        }

        public override string ToString()
        {
            return DocumentVersion.ToString();
        }
    }
}
