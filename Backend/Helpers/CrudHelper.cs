using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Dtos;
using Backend.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Helpers
{
    public class CrudHelper(AppDbContext dbContext, IMapper mapper)
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<InitDto> Init()
        {
            var carType = await _dbContext.CarType.ToListAsync();
            var engineSize = await _dbContext.EngineSize.ToListAsync();

            var init = new InitDto
            {
                CarType = _mapper.Map<List<CarType>>(carType),
                EngineSize = _mapper.Map<List<EngineSize>>(engineSize)
            };

            return init;
        }

        public async Task<IEnumerable<STNK>> GetAllStnk()
        {
            return await _dbContext.STNK.ToListAsync();
        }

        public async Task<object?> GetStnkByStnkNumber(string stnkNumber)
        {
            var stnk = await _dbContext.STNK
                .Join(_dbContext.Owner,
                    stnk => stnk.OwnerId,
                    owner => owner.Id,
                    (stnk, owner) => new
                    {
                        stnk, owner
                    })
                .Where(x => x.stnk.RegistrationNumber == stnkNumber)
                .FirstOrDefaultAsync();

            if (stnk == null) return null;

            return stnk;
        }

        public async Task InsertStnk(StnkDto stnk)
        {
            var x = await _dbContext.STNK
                .Where(x => x.RegistrationNumber == stnk.RegistrationNumber)
                .FirstOrDefaultAsync() ?? throw new Exception("Registration number has been registered!");

            x = new STNK
            {
                RegistrationNumber = await GetSequence(SequenceTypeEnum.STNK) ?? throw new Exception("Sequence error!"),
                OwnerId = await CreateOwner(stnk.OwnerNIK, stnk.OwnerName),
                CarName = stnk.CarName,
                CarType = stnk.CarType,
                CarPrice = stnk.CarPrice,
                LastTaxPrice = stnk.LastTaxPrice,
                AddedBy = "",
                AddedDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _dbContext.Add(x);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CreateOwner(string nik, string name)
        {
            var owner = await _dbContext.Owner
                .Where(x => x.NIK == nik)
                .FirstOrDefaultAsync();

            if (owner != null) return owner.Id;

            var sequence = await GetSequence(SequenceTypeEnum.NIK) ?? throw new Exception("Sequence error!");

            owner = new Owner { Name = name, NIK = sequence };
            _dbContext.Owner.Add(owner);
            await _dbContext.SaveChangesAsync();

            return owner.Id;
        }

        public async Task<string?> GetSequence(SequenceTypeEnum type)
        {
            var sequenceType = await _dbContext.SequenceType
                .Where(x => x.Id == (int)type)
                .FirstOrDefaultAsync();

            var sequence = await _dbContext.Sequence
                .Where(x => x.TypeId == (int)type)
                .FirstOrDefaultAsync();

            if (sequence == null) return null;

            sequence.CurrentSequence = sequence.CurrentSequence + 1;
            await _dbContext.SaveChangesAsync();

            return $"{sequenceType?.Pattern} {sequence.CurrentSequence}";
        }
    }
}
