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
        Task<StnkUpdateReadDto?> GetStnkFullAsync(string registrationNumber);
        Task<Stnk?> GetStnkAsync(string registrationNumber);
        Task<int> GetOwnerIdAsync(string name);
        Task<Owner?> GetOwnerAsync(int id);
        Task<int> GetCurrentCarNumber(int ownerId, string registrationNumber);
        Task InsertStnk(StnkInsertReadDto stnk, string registrationNumber, int ownerId, decimal tax);
        Task<int> InsertOwner(string name, string sequence);
        Task<Stnk> UpdateStnkAsync(StnkUpdateWriteDto stnkInput, Stnk stnk, decimal tax);
        Task SaveChangesAsync();
    }
}
