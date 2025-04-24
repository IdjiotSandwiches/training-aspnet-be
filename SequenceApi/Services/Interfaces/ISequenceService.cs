using SharedLibrary.Enums;

namespace SequenceApi.Services.Interfaces
{
    public interface ISequenceService
    {
        Task<string> GenerateSequence(SequenceTypeEnum type);
    }
}
