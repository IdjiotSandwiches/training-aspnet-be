using Backend.Dtos;
using Backend.Models;

namespace Backend.Repositories.Interfaces
{
    public interface IStnkRepository
    {
        Task<IEnumerable<CarType>> GetCarTypeAsync();
        Task<IEnumerable<EngineSize>> GetEngineSizeAsync();
        Task<IEnumerable<Stnk>> GetAllStnkAsync();
        Task<CarType> GetCarTypeAsync(int id);
        Task<EngineSize> GetEngineSizeAsync(int id);
        Task<StnkUpdateReadDto> GetStnkByRegistrationNumberAsync(string registrationNumber);
        Task<int> GetOwnerIdAsync(string name);
        Task<int> GetCurrentCarNumber(int ownerId, string registrationNumber);
        Task<bool> IsCarTypeEmptyAsync();
        Task<bool> IsEngineSizeEmptyAsync();
        Task<bool> IsStnkEmptyAsync(string registrationNumber);
        Task<bool> IsOwnerEmptyAsync(string name);
        Task InsertStnk(StnkInsertReadDto stnk, string registrationNumber, int ownerId, dynamic taxPercentage);
        Task<int> InsertOwner(string name, string sequence);
        Task<Stnk> UpdateStnkAsync(StnkUpdateWriteDto stnkInput, string registrationNumber, dynamic taxPercentage);
        Task SaveChangesAsync();
    }
}
