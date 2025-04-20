using Backend.Dtos;
using Backend.Enums;

namespace Backend.Services.Interfaces
{
    public interface IStnkService
    {
        Task<InitDto> Init();
        Task<IEnumerable<AllStnkDto>> GetAllStnk();
        Task<object?> GetStnkByStnkNumber(string stnkNumber);
        Task InsertStnk(StnkInsertReadDto stnk);
        Task<int> CreateOwner(string name);
        Task<string> GetSequence(SequenceTypeEnum type);
    }
}
