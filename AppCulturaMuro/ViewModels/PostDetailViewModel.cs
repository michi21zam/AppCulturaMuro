using AppCulturaMuro.Models;
using System.Collections.Generic;

namespace AppCulturaMuro.ViewModels
{
    public class PostDetailViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
    }
}