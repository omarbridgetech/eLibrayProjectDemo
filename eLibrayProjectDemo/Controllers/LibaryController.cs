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

        public IActionResult Index(int pageIndex = 1, string? search = "", string? category = "", int ageRating = 8, string? sort = "newest")
        {
            IQueryable<Book> query = context.Books;

            // Search functionality
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Title.Contains(search) || p.Author.Contains(search) || p.Publisher.Contains(search));
            }

            // Filter by category
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category == category);
            }

            // Filter by age rating
            if (ageRating != 8) // Default is 8 (All Ratings)
            {
                query = query.Where(p => p.AgeRating == ageRating);
            }

            // Sort functionality
            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.PriceBuy),
                "price_disc" => query.OrderByDescending(p => p.PriceBuy),
                _ => query.OrderByDescending(p => p.EbookId) // Default: newest
            };

            // Pagination
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            // Fetch books
            var books = query.ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.PageIndex = pageIndex;
            ViewBag.Books = books;

            // Return the same model
            return View(new LibarySearchModel
            {
                Search = search,
                Sort = sort,
                Category = category,
                AgeRating = ageRating
            });
        }
    }
}
