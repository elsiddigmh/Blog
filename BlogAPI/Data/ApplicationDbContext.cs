using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category 
                {
                    Id = 1,
                    Name = "Tech",
                    Slug = "tech"
                },
                new Category
                {
                    Id = 2,
                    Name = "News",
                    Slug = "news"
                },
                new Category
                {
                    Id = 3,
                    Name = "Sports",
                    Slug = "sports"
                },
                new Category
                {
                    Id = 4,
                    Name = "Economy",
                    Slug = "economy"
                },
                new Category
                {
                    Id = 5,
                    Name = "Politics",
                    Slug = "politics"
                    
                }

                
            );
        }




    }
}
