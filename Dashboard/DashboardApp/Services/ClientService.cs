using System.Net.Http.Json;
using Dashboard.Shared.DTOs;

namespace DashboardApp.Services
{
    public interface IClientService
    {
        Task<List<ClientDto>> GetAllAsync();
        Task<ClientDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(ClientDto client);
        Task<bool> UpdateAsync(ClientDto client);
        Task<bool> DeleteAsync(int id);
    }

    public class ClientService : IClientService
    {
        private readonly HttpClient _http;

        public ClientService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ClientDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<ClientDto>>("api/clients") ?? new();
        }

        public async Task<ClientDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ClientDto>($"api/clients/{id}");
        }

        public async Task<bool> CreateAsync(ClientDto client)
        {
            var response = await _http.PostAsJsonAsync("api/clients", client);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(ClientDto client)
        {
            var response = await _http.PutAsJsonAsync($"api/clients/{client.Id}", client);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/clients/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
