using Microsoft.EntityFrameworkCore;
using StarterKit.Models;

namespace StarterKit.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<ProfileImage> ProfileImages => Set<ProfileImage>();
    }
}
