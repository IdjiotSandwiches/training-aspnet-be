using System.Text.Json;
using SharedLibrary.Dtos;
using SharedLibrary.Enums;
using OwnerApi.ApiClients.Interfaces;

namespace OwnerApi.ApiClients
{
    public class SequenceApiClient(HttpClient httpClient) : ISequenceApiClient
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<string?> GetSequence(SequenceTypeEnum type)
        {
            var response = await _httpClient.GetAsync($"api/Sequence?type={(int)type}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var apiResponse = JsonSerializer.Deserialize<ApiResponseDto<string>>(responseData, options);

            if (apiResponse == null)
                return null;

            return apiResponse.Data;
        }
    }
}
