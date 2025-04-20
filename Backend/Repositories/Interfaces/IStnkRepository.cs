using Backend.Dtos;
using Backend.Models;

namespace Backend.Repositories.Interfaces
{
    public interface IStnkRepository
    {
        Task<IEnumerable<CarType>> GetCarTypeAsync();
        Task<IEnumerable<EngineSize>> GetEngineSizeAsync();
        Task<bool> IsCarTypeEmptyAsync();
        Task<bool> IsEngineSizeEmptyAsync();
        Task<bool> IsStnkEmptyAsync(string registrationNumber);
        Task<bool> IsOwnerEmptyAsync(string name);
        Task<IEnumerable<Stnk>> GetAllStnkAsync();
        Task<StnkUpdateReadDto> GetStnkByRegistrationNumberAsync(string registrationNumber);
        void InsertStnk(Stnk stnk);
        int InsertOwner(string name, string sequence);
        Task<int> GetOwnerIdAsync(string name);
        Task SaveChangesAsync();
    }
}
