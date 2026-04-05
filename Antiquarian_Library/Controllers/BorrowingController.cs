using Antiquarian_Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Antiquarian_Library.Controllers
{
    [Authorize]
    public class BorrowingController : Controller
    {
        private readonly LocalDatabaseService _db;

        public BorrowingController(LocalDatabaseService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var borrowed = await _db.GetBorrowingBooksAsync();
            return View(borrowed);
        }
    }
}
