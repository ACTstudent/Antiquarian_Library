using Antiquarian_Library.Models;
using Antiquarian_Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Antiquarian_Library.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly LocalDatabaseService _db;

        public HomeController(LocalDatabaseService db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            var books = await _db.GetBooksAsync();
            var totalBooks = books.Sum(b => b.Total);
            var availableBooks = books.Sum(b => b.Available);

            ViewBag.TotalBooks = totalBooks;
            ViewBag.AvailableBooks = availableBooks;
            ViewBag.PendingRequests = (await _db.GetRequestsAsync()).Count(r => r.Status == "Pending");
            ViewBag.ApprovedBorrowings = (await _db.GetBorrowingBooksAsync()).Count(b => b.Status == "Active");
            ViewBag.LibraryEntriesToday = (await _db.GetEntryLogsAsync()).Count(e => e.Date.Date == DateTime.Today);

            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
