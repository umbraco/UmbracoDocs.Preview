using System.Collections.Generic;
using System.Linq;

namespace Umbraco.Docs.Preview.UI.Models
{
    public class UmbracoDocsTreeNode
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int Level { get; set; }
        public int Sort { get; set; }
        public bool HasChildren => Directories.Any();
        public List<UmbracoDocsTreeNode> Directories { get; set; }

        public string PhysicalPath { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
