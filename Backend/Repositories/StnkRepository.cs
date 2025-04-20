using Backend.Data;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class StnkRepository(AppDbContext dbContext) : IStnkRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<Stnk>> GetAllStnkAsync()
        {
            return await _dbContext.Stnk.ToListAsync();
        }

        public async Task<IEnumerable<CarType>> GetCarTypeAsync()
        {
            return await _dbContext.CarType.ToListAsync();
        }

        public async Task<IEnumerable<EngineSize>> GetEngineSizeAsync()
        {
            return await _dbContext.EngineSize.ToListAsync();
        }

        public async Task<int> GetOwnerIdAsync(string name)
        {
            return await _dbContext.Owner
                .Where(x => x.Name == name)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();
        }

        public Task<Stnk> GetStnkByRegistrationNumberAsync(string registrationNumber)
        {
            throw new NotImplementedException();
        }

        public Task<Owner> InsertOwnerAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Stnk> InsertStnkAsync(Stnk stnk)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsCarTypeEmptyAsync()
        {
            return await _dbContext.CarType.AnyAsync();
        }

        public async Task<bool> IsEngineSizeEmptyAsync()
        {
            return await _dbContext.EngineSize.AnyAsync();
        }

        public async Task<bool> IsOwnerEmptyAsync(string name)
        {
            return await _dbContext.Owner.AnyAsync(x => x.Name == name);
        }
    }
}
