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
            var success = await _db.ApproveRequestAsync(id);
            if (!success)
            {
                TempData["Error"] = "Cannot approve: An error occurred, or the request is no longer pending.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(string id)
        {
            var success = await _db.RejectRequestAsync(id);
            if (!success)
            {
                TempData["Error"] = "Cannot reject: An error occurred, or the request is no longer pending.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
