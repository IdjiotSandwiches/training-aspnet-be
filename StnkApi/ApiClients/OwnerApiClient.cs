using System.Text;
using System.Text.Json;
using SharedLibrary.Dtos;
using StnkApi.ApiClients.Interfaces;
using StnkApi.Dtos;

namespace StnkApi.ApiClients
{
    public class OwnerApiClient(HttpClient httpClient) : IOwnerApiClient
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<OwnerReadDto?> GetOwner(int id)
        {
            var response = await _httpClient.GetAsync($"api/Owner/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var apiResponse = JsonSerializer.Deserialize<ApiResponseDto<OwnerReadDto>>(responseData, options);

            if (apiResponse == null)
                return null;

            return apiResponse.Data;
        }

        public async Task<int> GetOwnerId(string name)
        {
            var response = await _httpClient.GetAsync($"api/Owner/name/{name}");
            if (!response.IsSuccessStatusCode)
                return 0;

            var responseData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var apiResponse = JsonSerializer.Deserialize<ApiResponseDto<int>>(responseData, options);

            if (apiResponse == null)
                return 0;

            return apiResponse.Data;
        }

        public async Task<int> InsertOwner(OwnerWriteDto owner)
        {
            var json = JsonSerializer.Serialize(new OwnerWriteDto{ Name = owner.Name });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Owner", content);
            if (!response.IsSuccessStatusCode)
                return 0;

            var responseData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var apiResponse = JsonSerializer.Deserialize<ApiResponseDto<int>>(responseData, options);

            if (apiResponse == null)
                return 0;

            return apiResponse.Data;
        }
    }
}
