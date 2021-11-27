using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Umbraco.Docs.Preview.UI.Models;
using Umbraco.Docs.Preview.UI.Services;


namespace Umbraco.Docs.Preview.UI.Controllers
{
    [Route("documentation")]
    public class DocumentationController : Controller
    {
        private readonly ILogger<DocumentationController> _log;
        private readonly IDocumentService _docs;
        private readonly IMarkdownService _md;

        public DocumentationController(
            ILogger<DocumentationController> log,
            IDocumentService docs,
            IMarkdownService md)
        {
            _log = log;
            _docs = docs;
            _md = md;
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
