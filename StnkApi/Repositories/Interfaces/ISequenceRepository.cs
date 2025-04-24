using StnkApi.Enums;
using StnkApi.Models;

namespace StnkApi.Repositories.Interfaces
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
