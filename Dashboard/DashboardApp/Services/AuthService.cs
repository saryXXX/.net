using System.Net.Http.Json;
using Blazored.LocalStorage;
using Dashboard.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace DashboardApp.Services
{
    public interface IAuthService
    {
        Task<bool> Login(LoginDto loginDto);
        Task Logout();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _http = http;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<bool> Login(LoginDto loginDto)
        {
            var response = await _http.PostAsJsonAsync("login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (authResponse == null || string.IsNullOrEmpty(authResponse.AccessToken))
            {
                return false;
            }

            await _localStorage.SetItemAsync("authToken", authResponse.AccessToken);
            await _localStorage.SetItemAsync("userEmail", loginDto.Email);
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(authResponse.AccessToken, loginDto.Email);

            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("userEmail");
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }
}
