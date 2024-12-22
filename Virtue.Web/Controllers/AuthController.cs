using Microsoft.AspNetCore.Mvc;
using Virtue.Web.Services;

namespace Virtue.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly FirebaseAuthService _firebaseAuthService;

        public AuthController(FirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return View();
            }

            try
            {
                var token = await _firebaseAuthService.LoginWithEmailAndPasswordAsync(email, password);

                // Store token in session or cookies for further use
                HttpContext.Session.SetString("FirebaseToken", token);

                // Redirect to a secure area (e.g., Dashboard or Home)
                return RedirectToAction("Dashboard", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Login failed: " + ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return View();
            }

            try
            {
                var token = await _firebaseAuthService.RegisterUserAsync(email, password);

                // Redirect to Login after successful registration
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Registration failed: " + ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("FirebaseToken");
            return RedirectToAction("Login");
        }
    }
}
