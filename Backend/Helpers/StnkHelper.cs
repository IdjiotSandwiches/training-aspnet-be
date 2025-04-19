using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Dtos;
using Backend.Enums;
using AutoMapper;

namespace Backend.Helpers
{
    public class StnkHelper(AppDbContext dbContext, IMapper mapper)
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<InitDto> Init()
        {
            if (!await _dbContext.CarType.AnyAsync() || !await _dbContext.EngineSize.AnyAsync()) 
                throw new Exception("Initialize failed!");

            var init = new InitDto
            {
                CarType = await _dbContext.CarType.ToListAsync(),
                EngineSize = await _dbContext.EngineSize.ToListAsync()
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

        public async Task InsertStnk(StnkInsertReadDto stnk)
        {
            var x = _mapper.Map<STNK>(new StnkWriteDto
            {
                RegistrationNumber = await GetSequence(SequenceTypeEnum.STNK) ?? throw new Exception("STNK Sequence error!"),
                OwnerId = await CreateOwner(stnk.OwnerName),
                CarName = stnk.CarName,
                CarType = stnk.CarType,
                CarPrice = stnk.CarPrice,
                LastTaxPrice = stnk.LastTaxPrice,
                AddedBy = "",
                AddedDate = DateOnly.FromDateTime(DateTime.Now)
            });

            _dbContext.Add(x);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CreateOwner(string name)
        {
            var owner = await _dbContext.Owner
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync();

            if (owner != null) return owner.Id;

            var sequence = await GetSequence(SequenceTypeEnum.NIK) ?? throw new Exception("NIK Sequence error!");

            owner = new Owner { Name = name, NIK = sequence };
            _dbContext.Owner.Add(owner);
            await _dbContext.SaveChangesAsync();

            return owner.Id;
        }

        public async Task<string> GetSequence(SequenceTypeEnum type)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var sequenceType = await _dbContext.SequenceType
                .Where(x => x.Id == (int)type)
                .FirstOrDefaultAsync() ?? throw new Exception("Invalid operation!");

            var sequence = await _dbContext.Sequence
                .Where(x => x.TypeId == (int)type)
                .FirstOrDefaultAsync();
            
            if (sequence == null)
            {
                sequence = new Sequence { TypeId = sequenceType.Id, CurrentSequence = 1 };
                _dbContext.Add(sequence);
            }
            else
                sequence.CurrentSequence++;

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return $"{sequenceType.Pattern} {sequence.CurrentSequence}";
        }
    }
}
