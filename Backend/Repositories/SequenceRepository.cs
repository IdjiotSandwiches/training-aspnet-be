using Backend.Data;
using Backend.Enums;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class SequenceRepository(AppDbContext dbContext) : ISequenceRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Sequence> GetSequenceAsync(SequenceTypeEnum type)
        {
            return await _dbContext.Sequence
                .Where(x => x.TypeId == ((int)type))
                .SingleAsync();
        }

        public async Task<SequenceType> GetSequenceTypeAsync(SequenceTypeEnum type)
        {
            return await _dbContext.SequenceType
                .Where(x => x.Id == ((int)type))
                .SingleAsync();
        }

        public long InsertSequence(SequenceType type)
        {
            var sequence = new Sequence { TypeId = type.Id, CurrentSequence = 1 };
            _dbContext.Add(sequence);
            return sequence.CurrentSequence;
        }

        public long UpdateSequence(Sequence sequence)
        {
            sequence.CurrentSequence++;
            return sequence.CurrentSequence;
        }

        public async Task<bool> IsSequenceEmptyAsync(SequenceTypeEnum type)
        {
            return !await _dbContext.Sequence.AnyAsync(x => x.TypeId == (int)type);
        }

        public async Task<bool> IsSequenceTypeEmptyAsync(SequenceTypeEnum type)
        {
            return !await _dbContext.SequenceType.AnyAsync(x => x.Id == (int)type);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
