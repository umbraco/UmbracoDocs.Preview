using System.IO;
using System.Text.RegularExpressions;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Umbraco.Docs.Preview.UI.Extensions;
using Umbraco.Docs.Preview.UI.MiscellaneousOurStuff;
using Umbraco.Docs.Preview.UI.Models;

namespace Umbraco.Docs.Preview.UI.Services
{
    public class MarkdownService : IMarkdownService
    {
        public const string RegEx = @"\[([^\]]+)\]\(([^)]+)\)"; // wat?

        public string RenderMarkdown(DocumentationVersion version)
        {
            var rawMarkdown = File.ReadAllText(version.FileSystemPath);

            var clean = Regex.Replace(
                rawMarkdown,
                RegEx,
                x => LinkEvaluator(x, version),
                RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseAbbreviations()
                .UseAutoIdentifiers(AutoIdentifierOptions.GitHub)
                .UseCitations()
                .UseCustomContainers()
                .UseDefinitionLists()
                .UseEmphasisExtras()
                .UseFigures()
                .UseFooters()
                .UseFootnotes()
                .UseGridTables()
                .UseMathematics()
                .UseMediaLinks()
                .UsePipeTables()
                .UseYamlFrontMatter()
                .UseListExtras()
                .UseTaskLists()
                .UseDiagrams()
                .UseAutoLinks()
                //.UseSyntaxHighlighting() 
                .Build();

            var transform = Markdown.ToHtml(clean, pipeline);

            // HACK: fix html img element paths, e.g. on Getting-Started page, could be pipeline extension.
            transform = transform.Replace("src=\"images/", $"src=\"{version.FolderRelativeUrl}images/".RemoveEmptyFolderParts());

            return transform;
        }

        private string LinkEvaluator(Match match, DocumentationVersion version)
        {
            string mdUrlTag = match.Groups[0].Value;
            string linkText = match.Groups[1].Value;
            string rawUrl = match.Groups[2].Value;

            //Escpae external URLs
            if (rawUrl.StartsWith("http") || rawUrl.StartsWith("https") || rawUrl.StartsWith("ftp"))
                return mdUrlTag;

            //Escape anchor links
            if (rawUrl.StartsWith("#"))
                return mdUrlTag;

            //Correct internal image links
            if (rawUrl.StartsWith("../images/"))
                return mdUrlTag.Replace("../images/", $"{version.FolderRelativeUrl}images/".RemoveEmptyFolderParts());

            if (rawUrl.StartsWith("images/"))
            {
                return mdUrlTag.Replace("images/", $"{version.FolderRelativeUrl}images/".RemoveEmptyFolderParts());
            }

            if (!rawUrl.StartsWith("/"))
            {
                var relativeUrl = $"{version.FolderRelativeUrl}{rawUrl}".RemoveEmptyFolderParts();
                mdUrlTag = $"[{linkText}]({rawUrl.Replace(rawUrl, relativeUrl)})";
            }

            if (rawUrl.EndsWith("index.md"))
                mdUrlTag = mdUrlTag.Replace("/index.md", "/");
            else
                mdUrlTag.TrimEnd('/');

            ////Need to ensure we dont append the image links as they 404 if we add altTemplate
            //if (AppendAltLessonLink && rawUrl.StartsWith("images/") == false)
            //{
            //    return mdUrlTag.Replace(rawUrl, string.Format("{0}?altTemplate=Lesson", rawUrl.EnsureNoDotsInUrl()));
            //}

            return mdUrlTag.Replace(rawUrl, rawUrl.EnsureNoDotsInUrl()).RemoveEmptyFolderParts();
        }
    }
}
