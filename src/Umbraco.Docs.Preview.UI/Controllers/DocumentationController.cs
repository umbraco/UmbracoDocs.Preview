using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Docs.Preview.UI.Models;
using Umbraco.Docs.Preview.UI.Services;

namespace Umbraco.Docs.Preview.UI.Controllers
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
            if (!$"{Request.Path}".EndsWith("/"))
            {
                return RedirectPermanent($"{Request.Path}/");
            }

            if (!_docs.TryFindMarkdownFile(slug, out var version))
            {
                return NotFound();
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

    }
}
