using AppCulturaMuro.Models;
using AppCulturaMuro.ViewModels;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace AppCulturaMuro.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // Hash password with SHA256
        private string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "");
            }
        }

        // Save user data in session
        private void SetSession(User user)
        {
            Session["UserId"] = user.Id;
            Session["Username"] = user.Username;
            Session["FullName"] = user.FullName;
        }

        // GET /Auth/Login
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserId"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var hash = HashPassword(vm.Password);
            var user = _db.Users
                .FirstOrDefault(u => u.Email == vm.Email && u.PasswordHash == hash);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View(vm);
            }

            SetSession(user);
            return RedirectToAction("Index", "Home");
        }

        // GET /Auth/Register
        [HttpGet]
        public ActionResult Register() => View();

        // POST /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            bool exists = _db.Users
                .Any(u => u.Email == vm.Email || u.Username == vm.Username);

            if (exists)
            {
                ModelState.AddModelError("", "Email or username already in use.");
                return View(vm);
            }

            var user = new User
            {
                Username = vm.Username,
                FullName = vm.FullName,
                Email = vm.Email,
                PasswordHash = HashPassword(vm.Password)
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return RedirectToAction("Login");
        }

        // POST /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}