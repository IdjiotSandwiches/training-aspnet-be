using Backend.Common;
using Backend.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IStnkService
    {
        Task<Result<InitDto>> Init();
        Task<Result<IEnumerable<AllStnkDto>>> GetAllStnk();
        Task<Result<StnkUpdateReadDto>> GetStnkByRegistrationNumber(string registrationNumber);
        Task<Result<StnkInsertReadDto>> InsertStnk(StnkInsertReadDto stnk);
        Task<Result<StnkUpdateReadDto>> UpdateStnk(string registrationNumber, StnkUpdateWriteDto stnk);
        Task<Result<decimal>> CalculateTax(int carTypeId, int engineSizeId, decimal carPrice, string ownerName, string registrationNumber);
    }
}
