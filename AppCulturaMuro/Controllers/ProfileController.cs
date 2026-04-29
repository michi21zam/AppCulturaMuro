using AppCulturaMuro.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace AppCulturaMuro.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET /Profile/Me
        public ActionResult Me()
        {
            var userId = Session["UserId"] as int?;
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var user = _db.Users
                .Include(u => u.Posts)
                .FirstOrDefault(u => u.Id == userId.Value);

            if (user == null)
                return RedirectToAction("Login", "Auth");

            // Sort posts in memory after loading
            user.Posts = user.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .ToList();

            return View(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}