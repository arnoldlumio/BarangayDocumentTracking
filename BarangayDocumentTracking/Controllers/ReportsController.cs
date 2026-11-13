using Microsoft.AspNetCore.Mvc;

namespace BarangayDocumentTracking.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
    }
}
