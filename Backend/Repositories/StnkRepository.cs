using AutoMapper;
using Backend.Common;
using Backend.Data;
using Backend.Dtos;
using Backend.Helpers;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class StnkRepository(AppDbContext dbContext, IMapper mapper) : IStnkRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

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

        public async Task<CarType> GetCarTypeAsync(int id)
        {
            return await _dbContext.CarType.FindAsync(id) ?? throw new Exception("Car type didn't exists!");
        }

        public async Task<EngineSize> GetEngineSizeAsync(int id)
        {
            return await _dbContext.EngineSize.FindAsync(id) ?? throw new Exception("Engine size didnt't exists!");
        }

        public async Task<int> GetCurrentCarNumber(int ownerId, string registrationNumber)
        {
            var carList = await _dbContext.Stnk
                .Where(x => x.OwnerId == ownerId)
                .ToListAsync();

            return carList.FindIndex(x => x.RegistrationNumber == registrationNumber) + 1;
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

        public async Task<int> InsertOwner(string name, string sequence)
        {
            var owner = new Owner { Name = name, NIK = sequence };
            _dbContext.Owner.Add(owner);
            await SaveChangesAsync();
            return owner.Id;
        }

        public async Task InsertStnk(StnkInsertReadDto stnkInput, string registrationNumber, int ownerId, dynamic taxPercentage)
        {
            var currentCarNumber = await GetCurrentCarNumber(ownerId, registrationNumber);

            var dto = _mapper.Map<StnkInsertWriteDto>(stnkInput);
            dto.RegistrationNumber = registrationNumber;
            dto.OwnerId = ownerId;
            dto.LastTaxPrice = StnkHelper.CalculateTax(stnkInput.CarPrice, taxPercentage.CarTypeTax, taxPercentage.EngineSizeTax, currentCarNumber);
            dto.AddedBy = "";
            dto.AddedDate = DateOnly.FromDateTime(DateTime.Now);

            var stnk = _mapper.Map<Stnk>(dto);
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

        public async Task<Stnk> UpdateStnkAsync(StnkUpdateWriteDto stnkInput, string registrationNumber, dynamic taxPercentage)
        {
            var stnk = await _dbContext.Stnk
                .Where(x => x.RegistrationNumber == registrationNumber)
                .SingleAsync();

            var currentCarNumber = await GetCurrentCarNumber(stnk.OwnerId, registrationNumber);

            stnk.LastTaxPrice = StnkHelper.CalculateTax(stnkInput.CarPrice, taxPercentage.CarTypeTax, taxPercentage.EngineSizeTax, currentCarNumber);
            stnk.ModifiedBy = "";
            stnk.ModifiedDate = DateOnly.FromDateTime(DateTime.Now);

            _mapper.Map(stnkInput, stnk);
            await SaveChangesAsync();

            return stnk;
        }
    }
}
