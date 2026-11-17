
using BarangayDocumentTracking.Data;
using BarangayDocumentTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarangayDocumentTracking.Controllers
{
    public class DocumentRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DocumentRequest/Index
        [HttpGet]
        public IActionResult Index()
        {
            var documents = _context.Documents
                .Where(x => x.Status == "Available")
                .OrderBy(x => x.DocumentName)
                .ToList();

            return View(documents);
        }

        // GET: DocumentRequest/AddDocumentFillup/1
        [HttpGet]
        public IActionResult AddDocumentFillup(int id)
        {
            var document = _context.Documents
                .FirstOrDefault(d =>
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
            // Check if selected document exists
            var document = await _context.Documents
                .FirstOrDefaultAsync(d =>
                    d.Id == model.DocumentId &&
                    d.Status == "Available");

            if (document == null)
            {
                return NotFound();
            }

            // Check submitted resident information
            if (!ModelState.IsValid)
            {
                // Return to the same fill-up page
                return View("AddDocumentFillup", document);
            }

            // Generate unique request number
            model.RequestNumber =
                "BR-" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

            // Set request details
            model.Status = "Pending";
            model.RequestDate = DateTime.Now;
            model.ProcessedDate = null;

            // Save request to database
            _context.DocumentRequests.Add(model);

            await _context.SaveChangesAsync();

            // Success message
            TempData["SuccessMessage"] =
                "Your document request has been submitted successfully! " +
                "Your Request Number is: " + model.RequestNumber;

            // Return to document list
            return RedirectToAction("Index");
        }
    }
}
