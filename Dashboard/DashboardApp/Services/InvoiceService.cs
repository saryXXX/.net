using System.Net.Http.Json;
using Dashboard.Shared.DTOs;

namespace DashboardApp.Services
{
    public interface IInvoiceService
    {
        Task<List<InvoiceDto>> GetAllAsync();
        Task<InvoiceDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(InvoiceDto invoice);
        Task<bool> ValidateAsync(int id);
        Task<bool> DeleteAsync(int id);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly HttpClient _http;

        public InvoiceService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<InvoiceDto>> GetAllAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<List<InvoiceDto>>("api/factures") ?? new();
            }
            catch { return new(); }
        }

        public async Task<InvoiceDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<InvoiceDto>($"api/factures/{id}");
            }
            catch { return null; }
        }

        public async Task<bool> CreateAsync(InvoiceDto invoice)
        {
            var response = await _http.PostAsJsonAsync("api/factures", invoice);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ValidateAsync(int id)
        {
            var response = await _http.PutAsync($"api/factures/{id}/validate", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/factures/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
