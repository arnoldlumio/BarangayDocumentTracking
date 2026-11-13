using Microsoft.AspNetCore.Mvc;

namespace BarangayDocumentTracking.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            // Check if user is logged in
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}
