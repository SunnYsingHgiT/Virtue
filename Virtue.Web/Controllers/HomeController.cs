using Microsoft.AspNetCore.Mvc;

namespace Virtue.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
