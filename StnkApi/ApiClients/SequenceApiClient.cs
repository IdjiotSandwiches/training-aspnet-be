using System.Text.Json;
using SharedLibrary.Dtos;
using SharedLibrary.Enums;
using SharedLibrary.Helpers;
using StnkApi.ApiClients.Interfaces;

namespace StnkApi.ApiClients
{
    public class SequenceApiClient(HttpClient httpClient) : ISequenceApiClient
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ApiResponseDto<object>> GetSequence(SequenceTypeEnum type)
        {
            var response = await _httpClient.GetAsync($"api/Sequence?type={(int)type}");
            if (!response.IsSuccessStatusCode)
            {
                return ErrorResponseHelper.ErrorResponse((int)response.StatusCode, "Failed!", "");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var apiResponse = JsonSerializer.Deserialize<ApiResponseDto<object>>(responseData, options);

            if (apiResponse == null)
                return ErrorResponseHelper.ErrorResponse(StatusCodes.Status500InternalServerError, "Failed to parse response!", "");

            return apiResponse;
        }
    }
}
