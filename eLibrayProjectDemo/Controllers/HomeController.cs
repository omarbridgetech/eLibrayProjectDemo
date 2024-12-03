using Microsoft.AspNetCore.Mvc;

namespace eLibrayProjectDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
