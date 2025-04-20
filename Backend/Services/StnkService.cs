using AutoMapper;
using Backend.Data;
using Backend.Dtos;
using Backend.Enums;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class StnkService(AppDbContext dbContext, IMapper mapper) : IStnkService
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

        public async Task<IEnumerable<AllStnkDto>> GetAllStnk()
        {
            return _mapper.Map<IEnumerable<AllStnkDto>>(await _dbContext.Stnk.ToListAsync());
        }

        public async Task<StnkUpdateReadDto> GetStnkByStnkNumber(string stnkNumber)
        {
            var stnk = await _dbContext.Stnk
                .Join(_dbContext.Owner,
                    stnk => stnk.OwnerId,
                    owner => owner.Id,
                    (stnk, owner) => new
                    {
                        stnk,
                        owner
                    })
                .Where(x => x.stnk.RegistrationNumber == stnkNumber)
                .FirstOrDefaultAsync();

            if (stnk == null) return null;

            //return new StnkUpdateReadDto
            //{

            //};
        }

        public async Task InsertStnk(StnkInsertReadDto stnkInput)
        {
            var stnk = _mapper.Map<Stnk>(new StnkWriteDto
            {
                RegistrationNumber = await GetSequence(SequenceTypeEnum.STNK) ?? throw new Exception("STNK Sequence error!"),
                OwnerId = await CreateOwner(stnkInput.OwnerName),
                CarName = stnkInput.CarName,
                CarType = stnkInput.CarType,
                CarPrice = stnkInput.CarPrice,
                LastTaxPrice = stnkInput.LastTaxPrice,
                AddedBy = "",
                AddedDate = DateOnly.FromDateTime(DateTime.Now)
            });

            _dbContext.Add(stnk);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CreateOwner(string name)
        {
            var owner = await _dbContext.Owner
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync();

            if (owner != null) return owner.Id;

            var sequence = await GetSequence(SequenceTypeEnum.NIK) ?? 
                throw new Exception("NIK Sequence error!");

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

            return $"{sequenceType.Pattern}{sequence.CurrentSequence}";
        }

        Task<object?> IStnkService.GetStnkByStnkNumber(string stnkNumber)
        {
            throw new NotImplementedException();
        }
    }
}
