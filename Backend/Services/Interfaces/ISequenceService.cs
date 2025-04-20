using Backend.Common;
using Backend.Enums;

namespace Backend.Services.Interfaces
{
    public interface ISequenceService
    {
        Task<Result<string>> GenerateSequence(SequenceTypeEnum type);
    }
}
