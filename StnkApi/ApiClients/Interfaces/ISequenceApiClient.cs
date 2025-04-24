using SharedLibrary.Dtos;
using SharedLibrary.Enums;

namespace StnkApi.ApiClients.Interfaces
{
    public interface ISequenceApiClient
    {
        Task<ApiResponseDto<object>> GetSequence(SequenceTypeEnum type);
    }
}
