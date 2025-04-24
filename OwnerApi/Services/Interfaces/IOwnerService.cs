using OwnerApi.Dtos;

namespace OwnerApi.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<OwnerReadDto?> GetOwner(int id);
        Task<int> GetOwnerId(string name);
        Task InsertOwner(OwnerWriteDto owner);
    }
}
