using AppCulturaMuro.Models;
using AppCulturaMuro.ViewModels;
using System.IO;
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
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Auth");

            // Si hay búsqueda, redirige a Search
            if (!string.IsNullOrWhiteSpace(q))
                return RedirectToAction("Search", new { q });

            var posts = _db.Posts
                .Include("User")
                .Include("Comments")
                .Where(p => p.Published)
                .OrderByDescending(p => p.CreatedAt)
                .Take(25)
                .ToList();

            return View(posts);
        }

        // GET /Home/Search
        public ActionResult Search(string q)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Auth");

            if (string.IsNullOrWhiteSpace(q))
                return RedirectToAction("Index");

            var posts = _db.Posts
                .Include("User")
                .Include("Comments")
                .Where(p => p.Published && (
                    p.Title.Contains(q) ||
                    p.Content.Contains(q) ||
                    p.User.Username.Contains(q) ||
                    p.User.FullName.Contains(q)))
                .OrderByDescending(p => p.CreatedAt)
                .Take(20)
                .ToList();

            var users = _db.Users
                .Where(u =>
                    u.Username.Contains(q) ||
                    u.FullName.Contains(q))
                .Take(10)
                .ToList();

            var vm = new SearchResultViewModel
            {
                Query = q,
                Posts = posts,
                Users = users
            };

            return View(vm);
        }

        // GET /Home/Image?file=guid.jpg
        public ActionResult Image(string file)
        {
            if (string.IsNullOrWhiteSpace(file) || file.Contains(".."))
                return HttpNotFound();

            var path = Server.MapPath("~/App_Data/uploads/" + file);
            if (!System.IO.File.Exists(path))
                return HttpNotFound();

            var ext = Path.GetExtension(file).ToLower();
            var mime = ext == ".png" ? "image/png"
                     : ext == ".gif" ? "image/gif"
                     : "image/jpeg";

            return File(path, mime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}