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

        public DbSet<STNK> STNK { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<EngineSize> EngineSize { get; set; }
        public DbSet<CarType> CarType { get; set; }
        public DbSet<Sequence> Sequence { get; set; }
        public DbSet<SequenceType> SequenceType { get; set; }
    }
}
