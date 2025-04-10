using System.Text;
using System.Text.Json;
using CpCinemaBlazor.ApiRequest.Model;
using Microsoft.Extensions.Logging;
using static CpCinemaBlazor.ApiRequest.Model.User;
using static CpCinemaBlazor.ApiRequest.Model.Film;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using CpCinemaBlazor.ApiRequest.Services;
using System;
using Azure;
using Amazon.SimpleNotificationService.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Linq;
using static CpCinemaBlazor.Components.ChatComponent;

namespace CpCinemaBlazor.ApiRequest
{
    public class ApiRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiRequestService> _logger;
        private readonly IWebHostEnvironment _environment;


        public ApiRequestService(HttpClient httpClient, ILogger<ApiRequestService> logger, IWebHostEnvironment environment)
        {
            _httpClient = httpClient;
            _logger = logger;
            _environment = environment;
        }

        public async Task<List<UserDataShort>> GetUsersAsync()
        {
            //var url = "api/UsersLogins/GetAllUsers";

            try
            {
                var response = await _httpClient.GetAsync("http://localhost:5005/api/UserLogin/GetAllUsers");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning("Ответ от сервера пуст.");
                    return [];
                }

                var usersData = JsonSerializer.Deserialize<List<UserDataShort>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                 
                return usersData ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе");
                return [];
            }
        }

        public async Task<List<UserDataShort2>> GetUsers2Async()
        {
            //var url = "api/UsersLogins/GetAllUsers";

            try
            {
                var response = await _httpClient.GetAsync("http://localhost:5005/api/UserLogin/GetAllUsers");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning("Ответ от сервера пуст.");
                    return [];
                }

                var usersData = JsonSerializer.Deserialize<List<UserDataShort2>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return usersData ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе");
                return [];
            }
        }

        public async void EditUserAsync(UserProd user)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync("http://localhost:5005/api/UsersLogins/EditUser", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе");
            }
        }

        public async void DeleteUserAsync(int Id)
        {
            try
            {   
                HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:5005/api/UsersLogins/DeleteUser/{Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе");
            }
        }

        public async Task<string> CreateUserLoginAsync(AddUser user)
        {
            var response = await _httpClient.PostAsJsonAsync($"http://localhost:5005/api/UserLogin/Register", user);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Authorization failed: {errorContent}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            };
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, options);
            var token = tokenResponse.Token;

            return token;
        }
        
        public async Task<string> AuthorizationAsync(AuthUser auth)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5005/api/UsersLogins/Authorization", auth);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Authorization failed: {errorContent}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            };
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, options);
            var token = tokenResponse.Token;

            return token;
        }

        public class TokenResponse
        {
            public bool Status { get; set; }
            public string Token { get; set; }
        }       
    }
}

