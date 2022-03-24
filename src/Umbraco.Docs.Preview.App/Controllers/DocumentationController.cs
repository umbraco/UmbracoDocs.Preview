using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Docs.Preview.App.Models;
using Umbraco.Docs.Preview.App.Services;

namespace Umbraco.Docs.Preview.App.Controllers
{
    [Route("documentation")]
    public class DocumentationController : Controller
    {
        private readonly IDocumentService _docs;
        private readonly IMarkdownService _md;
        private readonly IDocumentationChangeNotifier _documentationChangeNotifier;

        public DocumentationController(
            IDocumentService docs,
            IMarkdownService md,
            IDocumentationChangeNotifier documentationChangeNotifier)
        {
            _docs = docs;
            _md = md;
            _documentationChangeNotifier = documentationChangeNotifier;
        }

        [HttpGet("should-reload")]
        public async Task<IActionResult> ShouldReload()
        {
            var reload = await _documentationChangeNotifier.WaitForChanges();

            return reload
                ? Ok()
                : Accepted();
        }

        [HttpGet("{**slug}")]
        public IActionResult Index(string slug)
        {
            if (!_docs.TryFindMarkdownFile(slug, out var version))
            {
                return NotFound();
            }

            if (ShouldRedirect(version, out var path))
            {
                return RedirectPermanent(path);
            }

            var model = new DocumentationViewModel
            {
                DocumentVersion = version,
                Navigation = _docs.GetDocsTree(),
                Alternates = _docs.GetAlternates(version).ToList(),
                Markup = _md.RenderMarkdown(version),
            };

            return View("DocumentationSubpage", model);
        }

        private bool ShouldRedirect(DocumentVersion version, out string url)
        {
            if (version.IsAlternate && $"{Request.Path}".EndsWith('/'))
            {
                url = $"{Request.Path}".TrimEnd('/');
                return true;
            }

            if (version.FileName.Equals("index.md") && !$"{Request.Path}".EndsWith("/"))
            {
                url = $"{Request.Path}/";
                return true;
            }

            var match = Regex.Match(Request.Path, $"(.*)index/?$");
            if(match.Success)
            {
                url = match.Groups[1].Value;
                return true;
            }

            url = null;
            return false;
        }

    }
}
