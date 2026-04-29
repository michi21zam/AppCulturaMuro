using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppCulturaMuro.Models
{
    public class Post
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public bool Published { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // Navigation
        public virtual ICollection<Comment> Comments { get; set; }
    }
}