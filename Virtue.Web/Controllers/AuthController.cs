using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Virtue.Web.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Virtue.Web.Helper;
using Virtue.UserService.Models;

namespace Virtue.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _userServiceClient;

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

        public AuthController(IHttpClientFactory clientFactory)
        {
            _userServiceClient = clientFactory.CreateClient("UserService");
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
                // Verify email exists in Firebase
                var userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(model.Email);

                if (userRecord == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(model);
                }

                // Retrieve user details from Virtue.UserService
                var response = await _userServiceClient.GetAsync($"users/{model.Email}");
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "User not found in database.");
                    return View(model);
                }

                var user = await response.Content.ReadFromJsonAsync<User>();
                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    return View(model);
                }

                HttpContext.Session.SetString("UserId", user.Id.ToString());
                return RedirectToAction("Dashboard", "Home");
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

            try
            {
                //Check if the email is already registered in Firebase
                try
                {
                    var existingUser = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(model.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError(string.Empty, "Email is already registered.");
                        return View(model);
                    }
                }
                catch (FirebaseAuthException)
                {
                    // Email not found in Firebase; continue with registration
                }

                // Create user in Firebase Authentication
                var userRecordArgs = new UserRecordArgs
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var firebaseUser = await FirebaseAuth.DefaultInstance.CreateUserAsync(userRecordArgs);

                // Create user in SQL Server via Virtue.UserService
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    PhoneNumber = model.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    Gender = model.Gender,
                    Address = model.Address,
                    DateOfBirth = model.DateOfBirth
                };

                var response = await _userServiceClient.PostAsJsonAsync("api/Users", user);

                if (!response.IsSuccessStatusCode)
                {
                    // Rollback Firebase user creation if SQL server fails
                    //await FirebaseAuth.DefaultInstance.DeleteUserAsync(firebaseUser.Uid);
                    var error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Error saving user to database: {error}");
                    return View(model);
                }

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Registration failed: " + ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login");
        }
    }
}

