using Backend.Data;
using Backend.Dtos;
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

        public async Task<StnkUpdateReadDto> GetStnkByRegistrationNumberAsync(string registrationNumber)
        {
            return await _dbContext.Stnk
                .Join(_dbContext.Owner,
                    stnk => stnk.OwnerId,
                    owner => owner.Id,
                    (stnk, owner) => new
                    {
                        stnk,
                        owner
                    })
                .Where(x => x.stnk.RegistrationNumber == registrationNumber)
                .Select(x => new StnkUpdateReadDto
                {
                    RegistrationNumber = x.stnk.RegistrationNumber,
                    CarName = x.stnk.CarName,
                    CarType = x.stnk.CarType,
                    EngineSize = x.stnk.EngineSize,
                    CarPrice = x.stnk.CarPrice,
                    LastTaxPrice = x.stnk.LastTaxPrice,
                    OwnerName = x.owner.Name,
                    OwnerNIK = x.owner.NIK
                })
                .SingleAsync();
        }

        public int InsertOwner(string name, string sequence)
        {
            var owner = new Owner { Name = name, NIK = sequence };
            _dbContext.Owner.Add(owner);
            return owner.Id;
        }

        public void InsertStnk(Stnk stnk)
        {
            _dbContext.Add(stnk);
        }

        public async Task<bool> IsCarTypeEmptyAsync()
        {
            return !await _dbContext.CarType.AnyAsync();
        }

        public async Task<bool> IsEngineSizeEmptyAsync()
        {
            return !await _dbContext.EngineSize.AnyAsync();
        }

        public async Task<bool> IsOwnerEmptyAsync(string name)
        {
            return !await _dbContext.Owner.AnyAsync(x => x.Name == name);
        }

        public async Task<bool> IsStnkEmptyAsync(string registrationNumber)
        {
            return !await _dbContext.Stnk.AnyAsync(x => x.RegistrationNumber == registrationNumber);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
