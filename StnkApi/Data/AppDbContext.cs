using Microsoft.EntityFrameworkCore;
using StnkApi.Models;

namespace StnkApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Stnk> Stnk { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<EngineSize> EngineSize { get; set; }
        public DbSet<CarType> CarType { get; set; }
    }
}
