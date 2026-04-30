using AppCulturaMuro.Models;
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

            var query = _db.Posts
                .Include("User")
                .Include("Comments")
                .Where(p => p.Published);

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

        // GET /Home/Image?file=guid.jpg
        public ActionResult Image(string file)
        {
            if (string.IsNullOrWhiteSpace(file) || file.Contains(".."))
                return HttpNotFound();

            var path = Server.MapPath("~/App_Data/uploads/" + file);
            if (!File.Exists(path))
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