using Microsoft.AspNetCore.Mvc;

namespace VetCareBackend.Presentation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
