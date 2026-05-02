using AppCulturaMuro.Models;
using System.Collections.Generic;

namespace AppCulturaMuro.ViewModels
{
    public class SearchResultViewModel
    {
        public string Query { get; set; }
        public List<Post> Posts { get; set; }
        public List<User> Users { get; set; }
    }
}