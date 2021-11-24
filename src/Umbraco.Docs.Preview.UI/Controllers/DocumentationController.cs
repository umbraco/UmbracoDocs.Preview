using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;

        public DocumentationController(
            ILogger<DocumentationController> log,
            IDocumentService docs,
            IMarkdownService md,
            IMemoryCache memoryCache)
        {
            _log = log;
            _docs = docs;
            _md = md;
            _memoryCache = memoryCache;
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

        [HttpDelete("caches")]
        public IActionResult InvalidateCaches()
        {
            _log.LogInformation("Clearing documentation caches");
            _memoryCache.Remove(nameof(_docs.GetDocsTree));
            return NoContent();
        }
    }
}
