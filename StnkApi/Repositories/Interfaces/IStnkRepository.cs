using StnkApi.Dtos;
using StnkApi.Models;

namespace StnkApi.Repositories.Interfaces
{
    public interface IStnkRepository
    {
        Task<IEnumerable<CarType>> GetCarTypeAsync();
        Task<IEnumerable<EngineSize>> GetEngineSizeAsync();
        Task<IEnumerable<Stnk>> GetStnksAsync();
        Task<CarType?> GetCarTypeAsync(int id);
        Task<EngineSize?> GetEngineSizeAsync(int id);
        Task<StnkUpdateReadDto?> GetStnkAsync(string registrationNumber);
        Task<int> GetOwnerIdAsync(string name);
        Task<int> GetCurrentCarNumber(int ownerId, string registrationNumber);
        Task InsertStnk(StnkInsertReadDto stnk, string registrationNumber, int ownerId, dynamic taxPercentage);
        Task<int> InsertOwner(string name, string sequence);
        Task<Stnk> UpdateStnkAsync(StnkUpdateWriteDto stnkInput, string registrationNumber, dynamic taxPercentage);
        Task SaveChangesAsync();
    }
}
