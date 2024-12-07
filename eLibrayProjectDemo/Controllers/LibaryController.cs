using eLibrayProjectDemo.Models;
using eLibrayProjectDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace eLibrayProjectDemo.Controllers
{
    public class LibaryController : Controller
    {
        private readonly AppDbContext context;
        private readonly int pageSize = 8;

        public LibaryController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index(int pageIndex)
        {
            //var books=context.Books.OrderByDescending(p=>p.EbookId).ToList();

            //ViewBag.Books=books;

            IQueryable<Book>query=context.Books;
            query = query.OrderByDescending(p => p.EbookId);

            //pagination functionalty
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            decimal count=query.Count();
            int totalPages=(int)Math.Ceiling(count/pageSize);
            query=query.Skip((pageIndex-1)*pageSize).Take(pageSize);

            var books = query.ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.PageIndex=pageIndex;
            ViewBag.Books=books;
            return View(books);

            
        }
    }
}
