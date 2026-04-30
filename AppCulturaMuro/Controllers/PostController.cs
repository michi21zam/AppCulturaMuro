using AppCulturaMuro.Models;
using AppCulturaMuro.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace AppCulturaMuro.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        private int? CurrentUserId => Session["UserId"] as int?;

        // GET /Post/Create
        [HttpGet]
        public ActionResult Create()
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "Auth");
            return View();
        }

        // POST /Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePostViewModel vm)
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "Auth");
            if (!ModelState.IsValid) return View(vm);

            string imageUrl = null;

            // Handle image upload
            if (vm.Image != null && vm.Image.ContentLength > 0)
            {
                var uploadsDir = Server.MapPath("~/App_Data/uploads");
                Directory.CreateDirectory(uploadsDir);
                var ext = Path.GetExtension(vm.Image.FileName);
                var fileName = Guid.NewGuid().ToString() + ext;
                vm.Image.SaveAs(Path.Combine(uploadsDir, fileName));
                imageUrl = "/Home/Image?file=" + fileName; // ← línea corregida
            }

            var post = new Post
            {
                Title = vm.Title,
                Content = vm.Content,
                ImageUrl = imageUrl,
                UserId = CurrentUserId.Value,
                Published = true
            };

            _db.Posts.Add(post);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // GET /Post/Detail/5
        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "Auth");

            var post = _db.Posts
                .Include("User")
                .Include("Comments.User")
                .FirstOrDefault(p => p.Id == id);

            if (post == null) return HttpNotFound();

            var vm = new PostDetailViewModel
            {
                Post = post,
                Comments = post.Comments.OrderBy(c => c.CreatedAt).ToList()
            };

            return View(vm);
        }

        // POST /Post/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(int postId, string content)
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "Auth");

            if (!string.IsNullOrWhiteSpace(content))
            {
                var comment = new Comment
                {
                    PostId = postId,
                    UserId = CurrentUserId.Value,
                    Content = content.Trim()
                };
                _db.Comments.Add(comment);
                _db.SaveChanges();
            }

            return RedirectToAction("Detail", new { id = postId });
        }

        // POST /Post/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "Auth");

            var post = _db.Posts.Find(id);
            if (post == null || post.UserId != CurrentUserId.Value)
                return new HttpStatusCodeResult(403);

            _db.Posts.Remove(post);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}