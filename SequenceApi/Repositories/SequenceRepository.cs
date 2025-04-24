using Microsoft.EntityFrameworkCore;
using SharedLibrary.Enums;
using SequenceApi.Data;
using SequenceApi.Repositories.Interfaces;
using SequenceApi.Models;

namespace SequenceApi.Repositories
{
    public class SequenceRepository(AppDbContext dbContext) : ISequenceRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Sequence?> GetSequenceAsync(SequenceTypeEnum type)
        {
            return await _dbContext.Sequence
                .Where(x => x.TypeId == (int)type)
                .SingleOrDefaultAsync();
        }

        public async Task<SequenceType?> GetSequenceTypeAsync(SequenceTypeEnum type)
        {
            return await _dbContext.SequenceType
                .Where(x => x.Id == (int)type)
                .SingleOrDefaultAsync();
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

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
