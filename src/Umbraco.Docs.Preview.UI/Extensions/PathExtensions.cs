using System.Text.RegularExpressions;

namespace Umbraco.Docs.Preview.UI.Extensions
{
    public static class PathExtensions
    {
        /// <summary>
        /// Replace // with /, could probably use a UrlBuilder etc but lazy quick win.
        /// </summary>
        public static string RemoveEmptyFolderParts(this string url)
        {
            return Regex.Replace(url, "/+", "/");
        }
    }
}
