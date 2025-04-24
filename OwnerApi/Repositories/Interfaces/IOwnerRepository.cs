using OwnerApi.Models;

namespace OwnerApi.Repositories.Interfaces
{
    public interface IOwnerRepository
    {
        Task<Owner?> GetOwnerAsync(int id);
        Task<int> GetOwnerIdAsync(string name);
        Task<int> InsertOwnerAsync(string name, string sequence);
        Task SaveChangesAsync();
    }
}
