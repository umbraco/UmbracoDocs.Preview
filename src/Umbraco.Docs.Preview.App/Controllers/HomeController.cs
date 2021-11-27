using Microsoft.AspNetCore.Mvc;

namespace Umbraco.Docs.Preview.App.Controllers
{

    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Documentation");
        }
    }
}
