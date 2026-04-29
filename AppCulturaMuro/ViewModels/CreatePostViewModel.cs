using System.ComponentModel.DataAnnotations;
using System.Web;

namespace AppCulturaMuro.ViewModels
{
    public class CreatePostViewModel
    {
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }

        public HttpPostedFileBase Image { get; set; }
    }
}