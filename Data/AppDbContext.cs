using Microsoft.EntityFrameworkCore;
using CamelRegistry.Models;

namespace CamelRegistry.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Camel> Camels { get; set; }
    }
}
