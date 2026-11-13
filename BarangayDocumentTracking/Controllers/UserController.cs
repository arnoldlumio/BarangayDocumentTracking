using BarangayDocumentTracking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarangayDocumentTracking.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }


        // =========================================
        // RESIDENT DASHBOARD
        // =========================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Check login
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }


            // Check role
            if (HttpContext.Session.GetString("Role") != "Resident")
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }


            var documents = await _context.Documents
                .Where(x => x.Status == "Available")
                .OrderBy(x => x.DocumentName)
                .ToListAsync();


            ViewBag.Documents = documents;


            var announcements = await _context.Announcements
                .Where(x => x.Status == "Published")
                .OrderByDescending(x => x.DatePosted)
                .ToListAsync();


            ViewBag.Announcements = announcements;


            return View();
        }


        // =========================================
        // RESIDENT ANNOUNCEMENTS
        // =========================================

        [HttpGet]
        public async Task<IActionResult> Announcements()
        {
            // Check login
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }


            // Check role
            if (HttpContext.Session.GetString("Role") != "Resident")
            {
                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }


            var announcements = await _context.Announcements
                .Where(x => x.Status == "Published")
                .OrderByDescending(x => x.DatePosted)
                .ToListAsync();


            return View(announcements);
        }
    }
}