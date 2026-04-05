using Antiquarian_Library.Models;
using Antiquarian_Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Antiquarian_Library.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly LocalDatabaseService _db;

        public BookController(LocalDatabaseService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _db.GetBooksAsync();
            return View(books);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                await _db.CreateBookAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var book = await _db.GetBookAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Book book)
        {
            if (id != book.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _db.UpdateBookAsync(id, book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _db.GetBookAsync(id);
            if (book != null)
            {
                await _db.DeleteBookAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
