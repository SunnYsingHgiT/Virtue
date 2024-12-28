using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Virtue.Web.Helper;
using Virtue.Web.Models;

namespace Virtue.Web.Controllers
{
    public class AuthController : Controller
    {
        static AuthController()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(FirestoreHelper.GetCredentialFilePath()),
                    ProjectId = "virtue-e759c"
                });
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);  
            }

            try
            {
                // Your login logic using model.Email and model.Password
                var email = model.Email;
                var password = model.Password;

                var userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);

                if (userRecord == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(model);  
                }

                HttpContext.Session.SetString("FirebaseUserId", userRecord.Uid);  

                return RedirectToAction("Home", "index"); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Login failed: " + ex.Message);
                return View(model);  
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                // Check if the email is already in use
                var existingUser = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email is already registered.");
                    return View();
                }
            }
            catch (FirebaseAuthException ex)
            {
                // FirebaseAuthException means the email is not registered, which is fine for creating a new user.
                // Continue with the registration process.
            }

            try
            {
                // Create a new user with email and password
                var userRecordArgs = new UserRecordArgs
                {
                    Email = model.Email,
                    Password = model.Password
                };

                // Call Firebase Admin SDK to create the user
                var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userRecordArgs);

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                // Catch any errors related to Firebase or registration process
                ModelState.AddModelError(string.Empty, "Registration failed: " + ex.Message);
                return View();
            }
        }


        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("FirebaseUserId");
            return RedirectToAction("Login");
        }
    }
}
