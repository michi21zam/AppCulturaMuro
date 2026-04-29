using System.Data.Entity;

namespace AppCulturaMuro.Models
{
    public class AppDbContext : DbContext
    {
        // "DefaultConnection" matches Web.config
        public AppDbContext() : base("DefaultConnection") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicit table names
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Comment>().ToTable("Comments");

            // Avoid multiple cascade paths error
            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}