using eLibrayProjectDemo.Models;
using eLibrayProjectDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace eLibrayProjectDemo.Controllers
{
    [Route("/Admin/[controller]/{action=Register}/{id?}")]
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
        public IActionResult Index(int pageIndex,string? search,string? column,string? orderBy)
        {
            IQueryable<Book> query = contex.Books;

            //search func...

            if(search != null)
            {
                query=query.Where(p=>p.Title.Contains(search)||p.Category.Contains(search)||p.Author.Contains(search)||p.Publisher.Contains(search));
            }

            //Sort-func...
            string[] validColumns = { "Category", "IsBuyOnly", "AvailableCopies", "PriceBuy", "PriceBorrow", "Publisher", "Author", "Title", "EbookId" };
            string[] validOrderBy = {"desc","asc"};

            if (!validColumns.Contains(column)) { column = "EbookId"; }
            if (!validOrderBy.Contains(orderBy)) { orderBy = "desc"; }
            //1. sort-by-title
            if(column== "Title")
            {
                if (orderBy == "asc") { query = query.OrderBy(p => p.Title); }
                else { query = query.OrderByDescending(p => p.Title); }
            }
            //2. sort-by-Catagory
            else if (column == "Category")
            {
                if (orderBy == "asc") { query = query.OrderBy(p => p.Category); }
                else { query = query.OrderByDescending(p => p.Category); }
            }
            //3.sort-by-IsBuyOnly
            else if (column == "IsBuyOnly")
            {
                if (orderBy == "asc") { query = query.OrderBy(p => p.IsBuyOnly); }
                else { query = query.OrderByDescending(p => p.IsBuyOnly); }
            }
            //4.sort-by-PriceBuy
            else if (column == "PriceBuy")
            {
                if (orderBy == "asc") { query = query.OrderBy(p => p.PriceBuy); }
                else { query = query.OrderByDescending(p => p.PriceBuy); }
            }
            //5.sort-by-AvailableCopies
            else if (column == "AvailableCopies")
            {
                if (orderBy == "asc") { query = query.OrderBy(p => p.AvailableCopies); }
                else { query = query.OrderByDescending(p => p.AvailableCopies); }
            }
            //6.sort-by-PriceBorow
            else if (column == "PriceBorrow")
            {
                if (orderBy == "asc") { query = query.OrderBy(p => p.PriceBorrow); }
                else { query = query.OrderByDescending(p => p.PriceBorrow); }
            }
            //7.sort-by-Author
            else if (column == "Author")
            {
                if (orderBy == "asc") { query = query.OrderBy(p => p.Author); }
                else { query = query.OrderByDescending(p => p.Author); }
            }
            //8.sort-by-Publisher
            else if (column == "Publisher")
            {
                if (orderBy == "asc") { query = query.OrderBy(p => p.Publisher); }
                else { query = query.OrderByDescending(p => p.Publisher); }
            }
            //9.sort-by-Id
            else if (column == "EbookId")
            {
                if (orderBy == "asc") { query = query.OrderBy(p => p.EbookId); }
                else { query = query.OrderByDescending(p => p.EbookId); }
            }





            //query = query.OrderByDescending(p => p.EbookId);


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

            ViewData["Column"] = column;
            ViewData["OrderBy"]=orderBy;
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

            // Save additional format files
            string bookDirectory = Path.Combine(environment.WebRootPath, "Books-files", Guid.NewGuid().ToString());
            Directory.CreateDirectory(bookDirectory);

            string? pdfPath = null, epubPath = null, mobiPath = null, f2bPath = null;

            if (bookDto.PdfFile != null)
            {
                pdfPath = Path.Combine(bookDirectory, bookDto.PdfFile.FileName); // Retain original file name
                using (var stream = new FileStream(pdfPath, FileMode.Create))
                {
                    bookDto.PdfFile.CopyTo(stream);
                }
            }

            if (bookDto.EpubFile != null)
            {
                epubPath = Path.Combine(bookDirectory, bookDto.EpubFile.FileName); // Retain original file name
                using (var stream = new FileStream(epubPath, FileMode.Create))
                {
                    bookDto.EpubFile.CopyTo(stream);
                }
            }

            if (bookDto.MobiFile != null)
            {
                mobiPath = Path.Combine(bookDirectory, bookDto.MobiFile.FileName); // Retain original file name
                using (var stream = new FileStream(mobiPath, FileMode.Create))
                {
                    bookDto.MobiFile.CopyTo(stream);
                }
            }

            if (bookDto.F2bFile != null)
            {
                f2bPath = Path.Combine(bookDirectory, bookDto.F2bFile.FileName); // Retain original file name
                using (var stream = new FileStream(f2bPath, FileMode.Create))
                {
                    bookDto.F2bFile.CopyTo(stream);
                }
            }

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
                PdfFilePath = pdfPath,
                EpubFilePath = epubPath,
                MobiFilePath = mobiPath,
                F2bFilePath = f2bPath

            };
            //Add the book to DB-Table
            contex.Books.Add(book);
            //save the changes
            contex.SaveChanges();   

            return RedirectToAction("Register", "Book");
        }

        public IActionResult EditBook(int id)
        {
            //find the book to edit
            var book = contex.Books.Find(id);

            if (book == null) {
                return RedirectToAction("Register", "Book");
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
                return RedirectToAction("Register", "Book");

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
            return RedirectToAction("Register", "Book");




        }

        public IActionResult DeleteBook(int id)
        {
            //find the book to edit
            var book = contex.Books.Find(id);

            if (book == null)
            {
                return RedirectToAction("Register", "Book");
            }

            string imageFullPath = environment.WebRootPath + "/Books-cover/" + book.CoverImagePath;
            System.IO.File.Delete(imageFullPath);
            contex.Books.Remove(book);
            contex.SaveChanges(true);
            return RedirectToAction("Register", "Book");



        }


        public IActionResult DiscountBook(int id)
        {
            // Find the book to apply a discount
            var book = contex.Books.Find(id);
            if (book == null)
            {
                return RedirectToAction("Register", "Book");
            }

            // Create a BookDto and populate it with the current data
            var bookDto = new BookDto()
            {
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                Formats = book.Formats,
                PriceBuy = book.PriceBuy, // Current price
                AvailableCopies = book.AvailableCopies,
                Category = book.Category,
                Description = book.Description,
                PublicationYear = book.PublicationYear,
                IsBuyOnly = book.IsBuyOnly,
                AgeRating = book.AgeRating,
            };

            ViewData["EbookId"] = book.EbookId;
            ViewData["CurrentPrice"] = book.PriceBuy; // Display the current price in the view
            return View(bookDto);
        }

        [HttpPost]
        public IActionResult DiscountBook(int id, BookDto bookDto)
        {
            var book = contex.Books.Find(id);
            if (book == null)
            {
                return RedirectToAction("Register", "Book");
            }

            if (!ModelState.IsValid)
            {
                ViewData["EbookId"] = book.EbookId;
                ViewData["CurrentPrice"] = book.PriceBuy;
                return View(bookDto);
            }

            // Check if the new price is less than the current price
            if (bookDto.PriceBuy >= book.PriceBuy)
            {
                ModelState.AddModelError("PriceBuy", "The new price must be less than the current price.");
                ViewData["EbookId"] = book.EbookId;
                ViewData["CurrentPrice"] = book.PriceBuy;
                return View(bookDto);
            }

            // Update the book's price and discount details
            book.PreviousPrice = book.PriceBuy; // Save the original price
            book.PriceBuy = bookDto.PriceBuy;   // Apply the new discounted price
            book.DiscountStartDate = DateTime.Now; // Set the discount start date

            // Save the changes
            contex.SaveChanges();
            return RedirectToAction("Register", "Book");
        }










    }
}
