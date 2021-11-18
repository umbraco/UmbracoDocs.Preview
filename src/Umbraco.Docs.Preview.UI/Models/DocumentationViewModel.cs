using System.Collections.Generic;

namespace Umbraco.Docs.Preview.UI.Models
{
    public class DocumentationViewModel
    {
        public DocumentationVersion Version { get; init; }
        public string Markup { get; init; }
        public UmbracoDocsTreeNode Navigation { get; init; }
        public List<DocumentationVersion> Alternates { get; init; }

        public bool IsCurrent(DocumentationVersion alternate)
        {
            return Version.FileSystemPath.Equals(alternate.FileSystemPath);
        }

        public override string ToString()
        {
            return Version.ToString();
        }
    }
}
