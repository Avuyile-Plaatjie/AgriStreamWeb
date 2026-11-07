using System.Security.Claims;
using AgriStream.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriStream.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AgriStreamContext _context;
        private readonly PasswordHasher<ApplicationUser> _hasher;

        public AccountController(AgriStreamContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<ApplicationUser>();
        }

        // --- SIGNUP (GET & POST) ---
        [HttpGet]
        public IActionResult Signup() => View();

        [HttpPost]
        public async Task<IActionResult> Signup(string fullName, string username, string password, string role)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                ViewBag.Error = "Username is already taken.";
                return View();
            }

            var user = new ApplicationUser
            {
                FullName = fullName,
                Username = username,
                Role = string.IsNullOrEmpty(role) ? "Farmer" : role
            };

            // Hash the password securely before saving to Azure SQL
            user.PasswordHash = _hasher.HashPassword(user, password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Redirect to login page after successful registration
            return RedirectToAction("Login");
        }

        // --- LOGIN (GET & POST) ---
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || _hasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            // Create claims including FullName and Role for personalized greeting
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName), // Stores user's name for greeting
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            // Direct user to home dashboard after successful login
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}