using StnkApi.Data;
using StnkApi.Enums;
using StnkApi.Repositories.Interfaces;
using StnkApi.Services.Interfaces;

namespace StnkApi.Services
{
    public class SequenceService(AppDbContext dbContext, ISequenceRepository repo) : ISequenceService
    {
        private readonly ISequenceRepository _repo = repo;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<string> GenerateSequence(SequenceTypeEnum type)
        {
            var sequenceType = await _repo.GetSequenceTypeAsync(type) ?? throw new NullReferenceException("");

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var sequence = await _repo.GetSequenceAsync(type);
                long currentSequence;

                if (sequence == null)
                    currentSequence = _repo.InsertSequence(sequenceType);
                else
                    currentSequence = _repo.UpdateSequence(sequence);

                await _repo.SaveChangesAsync();
                await transaction.CommitAsync();
                return $"{sequenceType.Pattern}{currentSequence}";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
    }
}
