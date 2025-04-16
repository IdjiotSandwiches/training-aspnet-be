using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Microsoft.Identity.Client;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<STNK> STNKs { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<EngineSize> EngineSizes { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
    }
}
