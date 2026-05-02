using AppCulturaMuro.Models;
using System.Linq;
using System.Web.Mvc;

namespace AppCulturaMuro.Controllers
{
    public class NotificationController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();
        private int? CurrentUserId => Session["UserId"] as int?;

        public ActionResult Index()
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "Auth");

            var notifications = _db.Notifications
                .Include("Post")
                .Where(n => n.UserId == CurrentUserId.Value)
                .OrderByDescending(n => n.CreatedAt)
                .Take(30)
                .ToList();

            return View(notifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkAllRead()
        {
            if (CurrentUserId == null)
                return RedirectToAction("Login", "Auth");

            var unread = _db.Notifications
                .Where(n => n.UserId == CurrentUserId.Value && !n.IsRead)
                .ToList();

            foreach (var n in unread)
                n.IsRead = true;

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}