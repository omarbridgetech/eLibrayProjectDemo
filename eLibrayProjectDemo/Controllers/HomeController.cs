using eLibrayProjectDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace eLibrayProjectDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext context;

        public HomeController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var books = context.Books
                .OrderByDescending(x => x.EbookId)
                .Take(4)
                .ToList();

            return View(books);
        }


    }
}
