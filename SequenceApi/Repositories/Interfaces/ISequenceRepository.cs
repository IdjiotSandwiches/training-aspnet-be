using SequenceApi.Models;
using SharedLibrary.Enums;

namespace SequenceApi.Repositories.Interfaces
{
    public interface ISequenceRepository
    {
        Task<Sequence?> GetSequenceAsync(SequenceTypeEnum type);
        Task<SequenceType?> GetSequenceTypeAsync(SequenceTypeEnum type);
        long InsertSequence(SequenceType type);
        long UpdateSequence(Sequence sequence);
        Task SaveChangesAsync();
    }
}
