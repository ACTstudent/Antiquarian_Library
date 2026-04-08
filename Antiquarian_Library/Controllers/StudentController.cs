using Antiquarian_Library.Models;
using Antiquarian_Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Antiquarian_Library.Controllers
{
    [AllowAnonymous]
    public class StudentController : Controller
    {
        private readonly LocalDatabaseService _db;

        public StudentController(LocalDatabaseService db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Entry()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Entry(string studentName, string studentId, string yearLevel)
        {
            var log = new EntryLog
            {
                StudentName = studentName,
                StudentId = studentId,
                YearLevel = yearLevel,
                Date = DateTime.Today,
                Time = DateTime.Now.ToString("hh:mm tt")
            };
            await _db.CreateEntryLogAsync(log);

            TempData["Message"] = "Your entry has been recorded.";
            return RedirectToAction(nameof(Entry));
        }

        [HttpGet]
        public async Task<IActionResult> Books()
        {
            var books = await _db.GetBooksAsync();
            var requests = await _db.GetRequestsAsync();
            ViewBag.PendingCounts = requests.Where(r => r.Status == "Pending")
                                            .GroupBy(r => r.BookTitle)
                                            .ToDictionary(g => g.Key, g => g.Count());
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> RequestBorrow(string bookTitle)
        {
            var books = await _db.GetBooksAsync();
            var requests = await _db.GetRequestsAsync();
            var book = books.FirstOrDefault(b => b.Title == bookTitle);
            
            var pendingCount = requests.Count(r => r.BookTitle == bookTitle && r.Status == "Pending");
            var actuallyAvailable = book != null ? book.Available - pendingCount : 0;

            if (book == null || actuallyAvailable <= 0)
            {
                TempData["Error"] = $"Cannot request '{bookTitle}'. It is currently out of stock.";
                return RedirectToAction(nameof(Books));
            }

            ViewBag.BookTitle = bookTitle;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestBorrow(BorrowingRequest req)
        {
            var books = await _db.GetBooksAsync();
            var requests = await _db.GetRequestsAsync();
            var book = books.FirstOrDefault(b => b.Title == req.BookTitle);
            
            var pendingCount = requests.Count(r => r.BookTitle == req.BookTitle && r.Status == "Pending");
            var actuallyAvailable = book != null ? book.Available - pendingCount : 0;

            if (book == null || actuallyAvailable <= 0)
            {
                TempData["Error"] = $"Cannot request '{req.BookTitle}'. It is currently out of stock.";
                return RedirectToAction(nameof(Books));
            }

            req.RequestDate = DateTime.Today;
            req.ReturnDate = DateTime.Today.AddDays(7);
            req.Status = "Pending";

            await _db.CreateRequestAsync(req);

            TempData["Message"] = $"Your request to borrow '{req.BookTitle}' has been submitted successfully.";
            return RedirectToAction(nameof(Books));
        }
    }
}
