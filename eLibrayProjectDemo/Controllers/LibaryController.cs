using eLibrayProjectDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace eLibrayProjectDemo.Controllers
{
    public class LibaryController : Controller
    {
        private readonly AppDbContext context;

        public LibaryController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            //var books=context.Books.OrderByDescending(p=>p.EbookId).ToList();

            //ViewBag.Books=books;

            var books = context.Books.OrderByDescending(p => p.EbookId).ToList();
            return View(books);

            return View();
        }
    }
}
