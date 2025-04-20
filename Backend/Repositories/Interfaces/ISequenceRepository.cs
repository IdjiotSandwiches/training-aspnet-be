using Backend.Enums;
using Backend.Models;

namespace Backend.Repositories.Interfaces
{
    public interface ISequenceRepository
    {
        Task<bool> IsSequenceTypeEmptyAsync(SequenceTypeEnum type);
        Task<bool> IsSequenceEmptyAsync(SequenceTypeEnum type);
        Task<Sequence> GetSequenceAsync(SequenceTypeEnum type);
        Task<SequenceType> GetSequenceTypeAsync(SequenceTypeEnum type);
        long InsertSequence(SequenceType type);
        long UpdateSequence(Sequence sequence);
        Task SaveChangesAsync();
    }
}
