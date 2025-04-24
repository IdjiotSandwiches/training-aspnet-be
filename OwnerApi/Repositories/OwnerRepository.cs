using Microsoft.EntityFrameworkCore;
using OwnerApi.Data;
using OwnerApi.Models;
using OwnerApi.Repositories.Interfaces;

namespace OwnerApi.Repositories
{
    public class OwnerRepository(AppDbContext dbContext) : IOwnerRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Owner?> GetOwnerAsync(int id)
        {
            return await _dbContext.Owner
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<int> GetOwnerIdAsync(string name)
        {
            return await _dbContext.Owner
                .Where(x => x.Name == name)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();
        }

        public async Task InsertOwnerAsync(string name, string sequence)
        {
            _dbContext.Owner.Add(new Owner
            {
                Name = name,
                Nik = sequence
            });

            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
