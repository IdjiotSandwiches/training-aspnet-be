using StnkApi.Enums;

namespace StnkApi.Services.Interfaces
{
    public interface ISequenceService
    {
        Task<string> GenerateSequence(SequenceTypeEnum type);
    }
}
