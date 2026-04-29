using AppCulturaMuro.Models;
using System.Linq;
using System.Web.Mvc;

namespace AppCulturaMuro.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET /Home/Index
        public ActionResult Index(string q)
        {
            // Guard: must be logged in
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Auth");

            var query = _db.Posts
                .Include("User")
                .Include("Comments")
                .Where(p => p.Published);

            // Search filter
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(p =>
                    p.Title.Contains(q) ||
                    p.Content.Contains(q) ||
                    p.User.Username.Contains(q));
            }

            var posts = query
                .OrderByDescending(p => p.CreatedAt)
                .Take(25)
                .ToList();

            ViewBag.Query = q;
            return View(posts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}