using SharedLibrary.Enums;
using StnkApi.Common;
using StnkApi.Dtos;

namespace StnkApi.Services.Interfaces
{
    public interface IStnkService
    {
        Task<InitDto> Init();
        Task<IEnumerable<AllStnkDto>> GetStnks();
        Task<StnkUpdateReadDto?> GetStnk(string registrationNumber);
        Task InsertStnk(StnkInsertReadDto stnk);
        Task<Result<StnkUpdateReadDto>> UpdateStnk(string registrationNumber, StnkUpdateWriteDto stnk);
        Task<Result<decimal>> CalculateTax(int carTypeId, int engineSizeId, decimal carPrice, string ownerName, string registrationNumber);
        Task<string?> GetSequence(SequenceTypeEnum type);
    }
}
