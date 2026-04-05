using Antiquarian_Library.Models;
using Antiquarian_Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Antiquarian_Library.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly LocalDatabaseService _db;

        public RequestController(LocalDatabaseService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _db.GetRequestsAsync();
            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(string id)
        {
            await _db.ApproveRequestAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
