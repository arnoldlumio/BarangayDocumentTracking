using BarangayDocumentTracking.Data;
using BarangayDocumentTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarangayDocumentTracking.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // =========================================================
        // DOCUMENT LIST
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var documents = await _context.Documents
                .OrderBy(x => x.DocumentName)
                .ToListAsync();

            return View(documents);
        }


        // =========================================================
        // CREATE - GET
        // =========================================================

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }


        // =========================================================
        // CREATE - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Document document)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }


            // Validate input
            if (!ModelState.IsValid)
            {
                return View(document);
            }


            // Check duplicate document code
            bool documentExists =
                await _context.Documents.AnyAsync(x =>
                    x.DocumentCode == document.DocumentCode);


            if (documentExists)
            {
                ModelState.AddModelError(
                    "DocumentCode",
                    "This document code already exists."
                );

                return View(document);
            }


            // Set created date
            document.CreatedAt = DateTime.Now;


            // Add document
            _context.Documents.Add(document);

            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] =
                document.DocumentName +
                " has been added successfully.";


            return RedirectToAction("Index");
        }


        // =========================================================
        // EDIT - GET
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var document =
                await _context.Documents
                    .FirstOrDefaultAsync(x => x.Id == id);


            if (document == null)
            {
                return NotFound();
            }


            return View(document);
        }


        // =========================================================
        // EDIT - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Document document)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }


            if (id != document.Id)
            {
                return NotFound();
            }


            if (!ModelState.IsValid)
            {
                return View(document);
            }


            // Check duplicate code
            bool duplicateCode =
                await _context.Documents.AnyAsync(x =>
                    x.DocumentCode == document.DocumentCode &&
                    x.Id != document.Id);


            if (duplicateCode)
            {
                ModelState.AddModelError(
                    "DocumentCode",
                    "Another document already uses this code."
                );

                return View(document);
            }


            var existingDocument =
                await _context.Documents
                    .FirstOrDefaultAsync(x => x.Id == id);


            if (existingDocument == null)
            {
                return NotFound();
            }


            // Update fields
            existingDocument.DocumentCode =
                document.DocumentCode;

            existingDocument.DocumentName =
                document.DocumentName;

            existingDocument.Description =
                document.Description;

            existingDocument.Requirements =
                document.Requirements;

            existingDocument.ProcessingTime =
                document.ProcessingTime;

            existingDocument.Fee =
                document.Fee;

            existingDocument.Status =
                document.Status;


            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] =
                existingDocument.DocumentName +
                " has been updated successfully.";


            return RedirectToAction("Index");
        }


        // =========================================================
        // DETAILS
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var document =
                await _context.Documents
                    .FirstOrDefaultAsync(x => x.Id == id);


            if (document == null)
            {
                return NotFound();
            }


            return View(document);
        }


        // =========================================================
        // DELETE
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var document =
                await _context.Documents
                    .FirstOrDefaultAsync(x => x.Id == id);


            if (document == null)
            {
                return NotFound();
            }


            _context.Documents.Remove(document);

            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] =
                document.DocumentName +
                " has been deleted successfully.";


            return RedirectToAction("Index");
        }
    }
}