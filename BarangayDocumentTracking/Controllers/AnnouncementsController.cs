using BarangayDocumentTracking.Data;
using BarangayDocumentTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarangayDocumentTracking.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementsController(
            ApplicationDbContext context)
        {
            _context = context;
        }


        /* =====================================================
           INDEX
           ===================================================== */

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }


            var announcements =
                _context.Announcements
                    .OrderByDescending(
                        x => x.DatePosted
                    )
                    .ToList();


            return View(announcements);
        }


        /* =====================================================
           CREATE - GET
           ===================================================== */

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }


            var announcement =
                new Announcement
                {
                    DatePosted =
                        DateTime.Now,

                    Status =
                        "Published"
                };


            return View(announcement);
        }


        /* =====================================================
           CREATE - POST
           ===================================================== */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Announcement announcement)
        {
            /* =================================================
               CHECK LOGIN
               ================================================= */

            if (
                HttpContext.Session.GetString(
                    "Username"
                ) == null
            )
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }


            /* =================================================
               VALIDATE MODEL
               ================================================= */

            if (!ModelState.IsValid)
            {
                return View(announcement);
            }


            /* =================================================
               SET CREATED DATE
               ================================================= */

            announcement.CreatedAt =
                DateTime.Now;


            /* =================================================
               ADD TO DATABASE
               ================================================= */

            _context.Announcements.Add(
                announcement
            );


            /* =================================================
               SAVE TO SQL SERVER
               ================================================= */

            await _context.SaveChangesAsync();


            /* =================================================
               MINIMUM 3-SECOND PROCESSING EXPERIENCE
               =================================================

               IMPORTANT:

               The announcement has ALREADY been saved
               successfully to SQL Server at this point.

               We are only delaying the redirect so the
               user can see the publishing animation.
            */

            await Task.Delay(
                TimeSpan.FromSeconds(3)
            );


            /* =================================================
               SUCCESS MESSAGE
               ================================================= */

            TempData["SuccessMessage"] =
                announcement.Title +
                " has been added successfully.";


            /* =================================================
               REDIRECT
               ================================================= */

            return RedirectToAction(
                "Index"
            );
        }


        /* =====================================================
           DETAILS
           ===================================================== */

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (
                HttpContext.Session.GetString(
                    "Username"
                ) == null
            )
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }


            var announcement =
                _context.Announcements
                    .FirstOrDefault(
                        x => x.Id == id
                    );


            if (announcement == null)
            {
                return NotFound();
            }


            return View(announcement);
        }


        /* =====================================================
           EDIT - GET
           ===================================================== */

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (
                HttpContext.Session.GetString(
                    "Username"
                ) == null
            )
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }


            var announcement =
                _context.Announcements
                    .FirstOrDefault(
                        x => x.Id == id
                    );


            if (announcement == null)
            {
                return NotFound();
            }


            return View(announcement);
        }


        /* =====================================================
           EDIT - POST
           ===================================================== */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            int id,
            Announcement announcement)
        {
            if (
                HttpContext.Session.GetString(
                    "Username"
                ) == null
            )
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }


            if (id != announcement.Id)
            {
                return NotFound();
            }


            if (!ModelState.IsValid)
            {
                return View(announcement);
            }


            var existingAnnouncement =
                _context.Announcements
                    .FirstOrDefault(
                        x => x.Id == id
                    );


            if (existingAnnouncement == null)
            {
                return NotFound();
            }


            existingAnnouncement.Title =
                announcement.Title;


            existingAnnouncement.Content =
                announcement.Content;


            existingAnnouncement.DatePosted =
                announcement.DatePosted;


            existingAnnouncement.Status =
                announcement.Status;


            _context.SaveChanges();


            TempData["SuccessMessage"] =
                existingAnnouncement.Title +
                " has been updated successfully.";


            return RedirectToAction(
                "Index"
            );
        }


        /* =====================================================
           DELETE
           ===================================================== */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (
                HttpContext.Session.GetString(
                    "Username"
                ) == null
            )
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }


            var announcement =
                _context.Announcements
                    .FirstOrDefault(
                        x => x.Id == id
                    );


            if (announcement == null)
            {
                return NotFound();
            }


            _context.Announcements.Remove(
                announcement
            );


            _context.SaveChanges();


            TempData["SuccessMessage"] =
                announcement.Title +
                " has been deleted successfully.";


            return RedirectToAction(
                "Index"
            );
        }
    }
}