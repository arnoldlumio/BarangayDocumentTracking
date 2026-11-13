using BarangayDocumentTracking.Data;
using BarangayDocumentTracking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarangayDocumentTracking.Controllers
{
    public class OutgoingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PdfService _pdfService;

        public OutgoingController(
            ApplicationDbContext context,
            PdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }


        // ==========================================
        // OUTGOING DOCUMENT LIST
        // ==========================================

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var documents = await _context.OutgoingDocuments
                .OrderByDescending(x => x.DateReleased)
                .ToListAsync();

            return View(documents);
        }


        // ==========================================
        // VIEW / GENERATE PDF
        // ==========================================

        public async Task<IActionResult> ViewDocument(int id)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var document =
                await _context.OutgoingDocuments
                    .FirstOrDefaultAsync(x => x.Id == id);


            if (document == null)
            {
                return NotFound();
            }


            // Generate customized PDF
            byte[] pdf =
                _pdfService.GenerateOutgoingDocument(
                    document
                );


            // Display PDF in browser
            return File(
                pdf,
                "application/pdf"
            );
        }
    }
}