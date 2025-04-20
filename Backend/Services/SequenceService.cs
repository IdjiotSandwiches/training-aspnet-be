using Backend.Common;
using Backend.Data;
using Backend.Enums;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services
{
    public class SequenceService(ISequenceRepository repo, AppDbContext dbContext, ILogger<SequenceService> logger) : ISequenceService
    {
        private readonly ISequenceRepository _repo = repo;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly ILogger<SequenceService> _logger = logger;

        public async Task<Result<string>> GenerateSequence(SequenceTypeEnum type)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            if (await _repo.IsSequenceTypeEmptyAsync(type))
                return Result<string>.Error(500, "Invalid operation!");

            try
            {
                var sequenceType = await _repo.GetSequenceTypeAsync(type);
                long currentSequence;

                if (await _repo.IsSequenceEmptyAsync(type))
                    currentSequence = _repo.InsertSequence(sequenceType);
                else
                {
                    var sequence = await _repo.GetSequenceAsync(type);
                    currentSequence = _repo.UpdateSequence(sequence);
                }

                await _repo.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result<string>.Success(200, "OK", $"{sequenceType.Pattern}{currentSequence}");
            } 
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error while generating sequence");
                return Result<string>.Error(500, "An error occurred!");
            }
        }
    }
}
