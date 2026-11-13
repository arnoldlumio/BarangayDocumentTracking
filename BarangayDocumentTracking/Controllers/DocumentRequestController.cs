using BarangayDocumentTracking.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BarangayDocumentTracking.Controllers
{
    public class DocumentRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocumentRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var documents = _context.Documents
                .Where(x => x.Status == "Available")
                .OrderBy(x => x.DocumentName)
                .ToList();

            return View(documents);
        }
    }
}