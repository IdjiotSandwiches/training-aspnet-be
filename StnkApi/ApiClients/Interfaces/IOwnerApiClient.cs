using StnkApi.Dtos;

namespace StnkApi.ApiClients.Interfaces
{
    public interface IOwnerApiClient
    {
        Task<int> GetOwnerId(string name);
        Task<OwnerReadDto?> GetOwner(int id);
        Task<int> InsertOwner(OwnerWriteDto owner);
    }
}
