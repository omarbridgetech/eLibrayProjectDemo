using eLibrayProjectDemo.Models;
using eLibrayProjectDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;

namespace eLibrayProjectDemo.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext contex;
        private readonly IWebHostEnvironment environment;

        private readonly int pageSize=5;

        public BookController(AppDbContext contex ,IWebHostEnvironment environment)
        {
            this.contex = contex;
            this.environment = environment;
        }
        public IActionResult Index(int pageIndex,string? search)
        {
            IQueryable<Book> query = contex.Books;

            //search func...

            if(search != null)
            {
                query=query.Where(p=>p.Title.Contains(search)||p.Category.Contains(search)||p.Author.Contains(search)||p.Publisher.Contains(search));
            }
            query = query.OrderByDescending(p => p.EbookId);

            //pagination functionality

            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            //find total number of books
            decimal count=query.Count();
            int totalPages=(int)Math.Ceiling(count/pageSize);
            query=query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var books= query.ToList();

            ViewData["PageIndex"]=pageIndex;
            ViewData["TotalPage"]=totalPages;
            ViewData["Search"] = search ?? "";
             return View(books);
        }

        public IActionResult AddBook()
        {
            return View();
        }

        


        [HttpPost]
        public IActionResult AddBook(BookDto bookDto)
        {
            if (bookDto.CoverImage == null)
            {
                ModelState.AddModelError("CoverImage", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(bookDto); // Pass the model back to the view
            }

            //save the image file 
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(bookDto.CoverImage!.FileName);
            string imageFullPath=environment.WebRootPath+"/Books-cover/" +newFileName;
            using (var stream = System.IO.File.Create(imageFullPath)) { 
                bookDto.CoverImage.CopyTo(stream);  
            }

            //save the new book into the database

            Book book = new Book()
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Publisher = bookDto.Publisher,
                Formats = bookDto.Formats,
                PriceBorrow=bookDto.PriceBorrow,
                PriceBuy=bookDto.PriceBuy,
                PublicationYear = bookDto.PublicationYear,
                AgeRating = bookDto.AgeRating,
                AvailableCopies = bookDto.AvailableCopies,
                IsBuyOnly = bookDto.IsBuyOnly,
                Category = bookDto.Category,
                Description = bookDto.Description,
                CoverImagePath = newFileName,
                AddedAt = DateTime.Now,

            };
            //Add the book to DB-Table
            contex.Books.Add(book);
            //save the changes
            contex.SaveChanges();   

            return RedirectToAction("Index", "Book");
        }

        public IActionResult EditBook(int id)
        {
            //find the book to edit
            var book = contex.Books.Find(id);

            if (book == null) {
                return RedirectToAction("Index", "Book");
            }

            var bookDto = new BookDto()
            {
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                Formats = book.Formats,
                PriceBorrow = book.PriceBorrow,
                PriceBuy = book.PriceBuy,
                PublicationYear = book.PublicationYear,
                AgeRating = book.AgeRating,
                AvailableCopies = book.AvailableCopies,
                IsBuyOnly = book.IsBuyOnly,
                Category = book.Category,
                Description = book.Description,
                //CoverImagePath = newFileName,
                //AddedAt = DateTime.Now,

            };
            ViewData["EbookId"] = book.EbookId;
            ViewData["CoverImagePath"]=book.CoverImagePath;
            ViewData["AddedAt"]=book.AddedAt.ToString("dd/MM/yyyy");

            return View(bookDto);

        }

        [HttpPost]
        public IActionResult EditBook(int id,BookDto bookDto)
        {
            var book = contex.Books.Find(id);
            if (book == null) {
                return RedirectToAction("Index", "Book");

            }

            if (!ModelState.IsValid)
            {
                ViewData["EbookId"] = book.EbookId;
                ViewData["CoverImagePath"] = book.CoverImagePath;
                ViewData["AddedAt"] = book.AddedAt.ToString("dd/MM/yyyy");
                return View(bookDto); // Pass the model back to the view
            }
            //update the image file if we have new submitted file
            string newFileName = book.CoverImagePath;

            if (bookDto.CoverImage != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(bookDto.CoverImage.FileName);
                string imageFullPath = environment.WebRootPath + "/Books-cover/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    bookDto.CoverImage.CopyTo(stream);
                }

                //delete the old image
                string oldImageFullPath = environment.WebRootPath + "/Books-cover/" + book.CoverImagePath;
                System.IO.File.Delete(oldImageFullPath);

            }

            //update the product in the DB

            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.Publisher = bookDto.Publisher;
            book.Formats = bookDto.Formats;
            book.PriceBorrow = bookDto.PriceBorrow;
            book.PriceBuy = bookDto.PriceBuy;
            book.PublicationYear = bookDto.PublicationYear;
            book.AgeRating = bookDto.AgeRating;
            book.AvailableCopies = bookDto.AvailableCopies;
            book.IsBuyOnly = bookDto.IsBuyOnly;
            book.Category = bookDto.Category;
            book.Description = bookDto.Description;
            book.CoverImagePath = newFileName;

            contex.SaveChanges();
            return RedirectToAction("Index", "Book");




        }

        public IActionResult DeleteBook(int id)
        {
            //find the book to edit
            var book = contex.Books.Find(id);

            if (book == null)
            {
                return RedirectToAction("Index", "Book");
            }

            string imageFullPath = environment.WebRootPath + "/Books-cover/" + book.CoverImagePath;
            System.IO.File.Delete(imageFullPath);
            contex.Books.Remove(book);
            contex.SaveChanges(true);
            return RedirectToAction("Index", "Book");



        }


    }
}
