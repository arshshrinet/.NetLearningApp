using LearningApp.Model;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.Repository
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}

