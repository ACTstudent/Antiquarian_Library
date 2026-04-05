using Antiquarian_Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Antiquarian_Library.Controllers
{
    [Authorize]
    public class EntryLogController : Controller
    {
        private readonly LocalDatabaseService _db;

        public EntryLogController(LocalDatabaseService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _db.GetEntryLogsAsync();
            return View(logs);
        }
    }
}
