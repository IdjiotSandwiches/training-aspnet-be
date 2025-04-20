using Backend.Models;

namespace Backend.Repositories.Interfaces
{
    public interface IStnkRepository
    {
        Task<IEnumerable<CarType>> GetCarTypeAsync();
        Task<IEnumerable<EngineSize>> GetEngineSizeAsync();
        Task<bool> IsCarTypeEmptyAsync();
        Task<bool> IsEngineSizeEmptyAsync();
        Task<IEnumerable<Stnk>> GetAllStnkAsync();
        Task<Stnk> GetStnkByRegistrationNumberAsync(string registrationNumber);
        Task<Stnk> InsertStnkAsync(Stnk stnk);
        Task<bool> IsOwnerEmptyAsync(string name);
        Task<Owner> InsertOwnerAsync(string name);
        Task<int> GetOwnerIdAsync(string name);
    }
}
