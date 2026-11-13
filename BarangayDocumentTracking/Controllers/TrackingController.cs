using Microsoft.AspNetCore.Mvc;

namespace BarangayDocumentTracking.Controllers
{
    public class TrackingController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}
