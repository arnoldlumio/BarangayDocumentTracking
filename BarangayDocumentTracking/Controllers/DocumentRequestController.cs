
using BarangayDocumentTracking.Data;
using BarangayDocumentTracking.Models;
using BarangayDocumentTracking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarangayDocumentTracking.Controllers
{
    public class DocumentRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        private readonly ILogger<DocumentRequestController> _logger;

        public DocumentRequestController(
            ApplicationDbContext context,
            EmailService emailService,
            ILogger<DocumentRequestController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        // GET: DocumentRequest/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var documents = await _context.Documents
                .Where(x => x.Status == "Available")
                .OrderBy(x => x.DocumentName)
                .ToListAsync();

            return View(documents);
        }

        // GET: DocumentRequest/AddDocumentFillup/1
        [HttpGet]
        public async Task<IActionResult> AddDocumentFillup(int id)
        {
            var document = await _context.Documents
                .FirstOrDefaultAsync(d =>
                    d.Id == id &&
                    d.Status == "Available");

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: DocumentRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentRequest model)
        {
            // 1. Find selected document
            var document = await _context.Documents
                .FirstOrDefaultAsync(d =>
                    d.Id == model.DocumentId &&
                    d.Status == "Available");

            if (document == null)
            {
                return NotFound();
            }

            // 2. Validate resident information
            if (!ModelState.IsValid)
            {
                return View("AddDocumentFillup", document);
            }

            // 3. Generate request number
            model.RequestNumber =
                "BR-" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

            // 4. Set default request details
            model.Status = "Pending";
            model.RequestDate = DateTime.Now;
            model.ProcessedDate = null;
            model.Document = null;

            // 5. Save request to SQL Server
            try
            {
                _context.DocumentRequests.Add(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to save document request for {Email}",
                    model.Email);

                TempData["ErrorMessage"] =
                    "Your request could not be saved. Please try again.";

                return RedirectToAction(
                    nameof(AddDocumentFillup),
                    new { id = model.DocumentId });
            }

            // 6. Send confirmation email to resident
            try
            {
                await _emailService.SendRequestConfirmationAsync(
                    model,
                    document);

                TempData["SuccessMessage"] =
                    "Your request was submitted successfully! " +
                    "A confirmation email was sent to " + model.Email;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Email delivery failed for request {RequestNumber} to {Email}",
                    model.RequestNumber,
                    model.Email);

                TempData["SuccessMessage"] =
                    "Your document request was saved successfully! " +
                    "Request Number: " + model.RequestNumber;

                TempData["EmailWarning"] =
                    "Your request was saved, but the confirmation email " +
                    "could not be sent. Please contact the barangay office.";
            }

            // 7. Return to document list
            return RedirectToAction(nameof(Index));
        }
    }
}
