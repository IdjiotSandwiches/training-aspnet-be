using Microsoft.EntityFrameworkCore;
using SequenceApi.Models;

namespace SequenceApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Sequence> Sequence { get; set; }
        public DbSet<SequenceType> SequenceType { get; set; }
    }
}
