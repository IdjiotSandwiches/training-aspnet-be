using AutoMapper;
using StnkApi.Models;
using Microsoft.EntityFrameworkCore;
using StnkApi.Data;
using StnkApi.Dtos;
using StnkApi.Helpers;
using StnkApi.Repositories.Interfaces;

namespace StnkApi.Repositories
{
    public class StnkRepository(AppDbContext dbContext, IMapper mapper) : IStnkRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<Stnk>> GetStnksAsync()
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

        public async Task<CarType?> GetCarTypeAsync(int id)
        {
            return await _dbContext.CarType.FindAsync(id);
        }

        public async Task<EngineSize?> GetEngineSizeAsync(int id)
        {
            return await _dbContext.EngineSize.FindAsync(id);
        }

        public async Task<int> GetCurrentCarNumber(int ownerId, string registrationNumber)
        {
            var carList = await _dbContext.Stnk
                .Where(x => x.OwnerId == ownerId)
                .ToListAsync();

            return registrationNumber == "" ? carList.Count + 1 : carList.FindIndex(x => x.RegistrationNumber == registrationNumber) + 1;
        }

        public async Task<int> GetOwnerIdAsync(string name)
        {
            return await _dbContext.Owner
                .Where(x => x.Name == name)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();
        }

        public async Task<StnkUpdateReadDto?> GetStnkFullAsync(string registrationNumber)
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
                    OwnerNik = x.owner.NIK
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Stnk?> GetStnkAsync(string registrationNumber)
        {
            return await _dbContext.Stnk
                .Where(x => x.RegistrationNumber == registrationNumber)
                .SingleOrDefaultAsync();
        }

        public async Task<int> InsertOwner(string name, string sequence)
        {
            var owner = new Owner { Name = name, NIK = sequence };
            _dbContext.Owner.Add(owner);
            await SaveChangesAsync();
            return owner.Id;
        }

        public async Task InsertStnk(StnkInsertReadDto stnkInput, string registrationNumber, int ownerId, decimal tax)
        {
            var dto = _mapper.Map<StnkInsertWriteDto>(stnkInput);
            dto.RegistrationNumber = registrationNumber;
            dto.OwnerId = ownerId;
            dto.LastTaxPrice = tax;
            dto.AddedBy = "";
            dto.AddedDate = DateOnly.FromDateTime(DateTime.Now);

            var stnk = _mapper.Map<Stnk>(dto);
            _dbContext.Add(stnk);
            await SaveChangesAsync();
        }

        public async Task<Stnk> UpdateStnkAsync(StnkUpdateWriteDto stnkInput, Stnk stnk, decimal tax)
        {
            stnk.LastTaxPrice = tax;
            stnk.ModifiedBy = "";
            stnk.ModifiedDate = DateOnly.FromDateTime(DateTime.Now);

            _mapper.Map(stnkInput, stnk);
            await SaveChangesAsync();

            return stnk;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Owner?> GetOwnerAsync(int id)
        {
            return await _dbContext.Owner.FindAsync(id);
        }
    }
}
