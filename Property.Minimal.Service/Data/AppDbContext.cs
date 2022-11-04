using Microsoft.EntityFrameworkCore;
using Property.Minimal.Service.Models;

namespace Property.Minimal.Service.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Properties> Properties { get; set; }
    }
}
