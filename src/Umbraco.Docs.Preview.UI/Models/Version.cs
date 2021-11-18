using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Umbraco.Docs.Preview.UI.Extensions;

namespace Umbraco.Docs.Preview.UI.Models
{
    public class DocumentationVersion
    {
        public string FileSystemPath { get; set; }

        public string[] RelativePathSegments { get; set; }
        public string FolderRelativeUrl => $"/documentation/{string.Join('/', RelativePathSegments)}/".RemoveEmptyFolderParts();

        public string FileName => Path.GetFileName(FileSystemPath);
        public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(FileSystemPath);
        public bool IsAlternate => FileName.Contains("v-pre") || Regex.IsMatch(FileName, @"-v\d+");

        public string Url => IsAlternate ? $"{FolderRelativeUrl}{FileNameWithoutExtension}" : FolderRelativeUrl;


        public DocumentationVersion(string root, string filename)
        {
            FileSystemPath = Path.Combine(root, filename);
            RelativePathSegments = new Uri(Path.GetDirectoryName(FileSystemPath)!)
                .Segments
                .Skip(new Uri(root).Segments.Length)
                .ToArray();
        }

        public DocumentationVersion(string root, string filename, params string[] parts)
        {
            FileSystemPath = Path.Combine(root, Path.Combine(parts), filename);
            RelativePathSegments = new Uri(Path.GetDirectoryName(FileSystemPath)!)
                .Segments
                .Skip(new Uri(root).Segments.Length)
                .ToArray();
        }
        
        public override string ToString()
        {
            return FileName;
        }
    }
}
