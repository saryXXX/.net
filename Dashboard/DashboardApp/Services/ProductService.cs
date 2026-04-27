using System.Net.Http.Json;
using Dashboard.Shared.DTOs;

namespace DashboardApp.Services
{
    public interface IProductService
    {
        Task<List<ProduitDto>> GetAllAsync();
        Task<ProduitDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(ProduitDto produit);
        Task<bool> UpdateAsync(ProduitDto produit);
        Task<bool> DeleteAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProduitDto>> GetAllAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<List<ProduitDto>>("api/produits") ?? new();
            }
            catch { return new(); }
        }

        public async Task<ProduitDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<ProduitDto>($"api/produits/{id}");
            }
            catch { return null; }
        }

        public async Task<bool> CreateAsync(ProduitDto produit)
        {
            var response = await _http.PostAsJsonAsync("api/produits", produit);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(ProduitDto produit)
        {
            var response = await _http.PutAsJsonAsync($"api/produits/{produit.Id}", produit);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/produits/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
