using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Persistance.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Platform> Platforms { get; set; }
    }
}
