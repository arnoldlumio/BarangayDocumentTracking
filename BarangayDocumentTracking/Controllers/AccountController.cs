using BarangayDocumentTracking.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarangayDocumentTracking.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in
            if (HttpContext.Session.GetString("Username") != null)
            {
                string role =
                    HttpContext.Session.GetString("Role");

                if (role == "Administrator")
                {
                    return RedirectToAction(
                        "Index",
                        "Dashboard");
                }

                if (role == "Resident")
                {
                    return RedirectToAction(
                        "Index",
                        "User");
                }
            }

            return View();
        }


        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            // =========================================
            // ADMINISTRATOR LOGIN
            // =========================================

            if (model.Username == "admin" &&
                model.Password == "admin123")
            {
                HttpContext.Session.SetString(
                    "Username",
                    model.Username);

                HttpContext.Session.SetString(
                    "Role",
                    "Administrator");


                return RedirectToAction(
                    "Index",
                    "Dashboard");
            }


            // =========================================
            // RESIDENT LOGIN
            // =========================================

            if (model.Username == "resident" &&
                model.Password == "resident123")
            {
                HttpContext.Session.SetString(
                    "Username",
                    model.Username);

                HttpContext.Session.SetString(
                    "Role",
                    "Resident");


                return RedirectToAction(
                    "Index",
                    "User");
            }


            // =========================================
            // INVALID LOGIN
            // =========================================

            ModelState.AddModelError(
                "",
                "Invalid username or password.");

            return View(model);
        }



        // =========================================
        // LOGOUT
        // =========================================

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Account");
        }




       
    }
}