using Microsoft.EntityFrameworkCore;
using OwnerApi.Models;

namespace OwnerApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Owner> Owner { get; set; }
    }
}
