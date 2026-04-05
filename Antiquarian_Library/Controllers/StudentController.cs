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
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> RequestBorrow(string bookTitle)
        {
            ViewBag.BookTitle = bookTitle;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestBorrow(BorrowingRequest req)
        {
            req.RequestDate = DateTime.Today;
            req.ReturnDate = DateTime.Today.AddDays(7);
            req.Status = "Pending";

            await _db.CreateRequestAsync(req);

            TempData["Message"] = $"Your request to borrow '{req.BookTitle}' has been submitted successfully.";
            return RedirectToAction(nameof(Books));
        }
    }
}
