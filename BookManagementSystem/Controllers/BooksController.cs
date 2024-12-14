using BookManagementSystem.Models;
using BookManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: /Books
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBooksAsync();
            if (books == null)
            {
                return View(new List<Book>());
            }
            return View(books);
        }

        // GET: /Books/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // GET: /Books/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Validate file extension
                    var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

                    if (!validExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ImageFile", "Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed.");
                    }
                    else
                    {
                        var fileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        book.ImageUrl = @"images\" + fileName; // Use the unique file name in the URL
                    }
                }

                // Check if the book already exists
                var existingBook = await _bookService.GetBookByIdAsync(book.Id);
                if (existingBook != null)
                {
                    // If the book exists, update it
                    existingBook.Title = book.Title;
                    existingBook.Author = book.Author;
                    existingBook.Genre = book.Genre;
                    existingBook.PublishedYear = book.PublishedYear;
                    existingBook.Price = book.Price;
                    existingBook.DiscountPercentage = book.DiscountPercentage;
                    existingBook.ImageUrl = book.ImageUrl; // Update the image URL

                    await _bookService.UpdateBookAsync(existingBook);
                }
                else
                {
                    // If it's a new book, add it
                    await _bookService.AddBookAsync(book);
                }

                return RedirectToAction(nameof(Index)); // Redirect to the Index page after successful save
            }

            // If model state is not valid, redisplay the form with errors
            return View(book);
        }

        // GET: /Books/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: /Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                var existingBook = await _bookService.GetBookByIdAsync(book.Id);
                if (existingBook == null)
                {
                    return NotFound();
                }

                // Update book details
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Genre = book.Genre;
                existingBook.PublishedYear = book.PublishedYear;
                existingBook.Price = book.Price;
                existingBook.DiscountPercentage = book.DiscountPercentage;

                // Handle image update
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Validate file extension
                    var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

                    if (!validExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ImageFile", "Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed.");
                    }
                    else
                    {
                        var fileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        // Update the image URL
                        existingBook.ImageUrl = "/images/" + fileName;
                    }
                }

                await _bookService.UpdateBookAsync(existingBook);

                return RedirectToAction(nameof(Index)); // Redirect to the Index page after successful save
            }

            // If model state is not valid, redisplay the form with errors
            return View(book);
        }

        // GET: /Books/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: /Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            // If there is an associated image, delete it from the file system
            if (!string.IsNullOrEmpty(book.ImageUrl))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            await _bookService.DeleteBookAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
