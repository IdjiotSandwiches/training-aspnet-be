using Backend.Common;
using Backend.Enums;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Services.Interfaces
{
    public interface ISequenceService
    {
        Task<Result<string>> GenerateSequence(SequenceTypeEnum type, IDbContextTransaction transaction);
    }
}
