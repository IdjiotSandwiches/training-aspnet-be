using Backend.Common;
using Backend.Dtos;
using Backend.Enums;

namespace Backend.Services.Interfaces
{
    public interface IStnkService
    {
        Task<Result<InitDto>> Init();
        Task<Result<IEnumerable<AllStnkDto>>> GetAllStnk();
        Task<Result<StnkUpdateReadDto>> GetStnkByRegistrationNumber(string registrationNumber);
        Task<Result<StnkInsertReadDto>> InsertStnk(StnkInsertReadDto stnk);
    }
}
