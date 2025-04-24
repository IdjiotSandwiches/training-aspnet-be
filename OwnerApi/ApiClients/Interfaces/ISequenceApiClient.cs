using SharedLibrary.Enums;

namespace OwnerApi.ApiClients.Interfaces
{
    public interface ISequenceApiClient
    {
        Task<string?> GetSequence(SequenceTypeEnum type);
    }
}
