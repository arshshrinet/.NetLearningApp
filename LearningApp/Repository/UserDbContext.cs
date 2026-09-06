using LearningApp.Model;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.Repository
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision/scale to avoid silent truncation warnings
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price).HasPrecision(18, 2);
                entity.Property(p => p.Rating).HasPrecision(3, 2);
            });
        }
    }
}

