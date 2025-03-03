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

namespace CpCinemaBlazor.ApiRequest
{
    public class ApiRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiRequestService> _logger;

        public ApiRequestService(HttpClient httpClient, ILogger<ApiRequestService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<UserDataShort>> GetUsersAsync()
        {
            var url = "api/UsersLogins/GetUsers";

            try
            {
                var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning("Ответ от сервера пуст.");
                    return [];
                }

                var usersData = JsonSerializer.Deserialize<List<UserDataShort>>(responseContent, new JsonSerializerOptions //
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
            var response = await _httpClient.PostAsJsonAsync($"http://localhost:5005/api/UsersLogins/CreateNewUserAndLogin", user);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Authorization failed: {errorContent}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // Игнорировать регистр свойств
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Сопоставлять snake_case с CamelCase
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
                PropertyNameCaseInsensitive = true, // Игнорировать регистр свойств
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Сопоставлять snake_case с CamelCase
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

        
        public async Task<FilmData> GetAllFilmsAsync()
        {
            var url = "api/FilmsGenres/getAllFilms";

            try
            {
                var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning("Ответ от сервера пуст.");
                    return new FilmData();
                }

                var filmData = JsonSerializer.Deserialize<FilmData>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


                return filmData ?? new FilmData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе");
                return new FilmData();
            }
        }

        public async Task<Film> CreateFilmAsync(AddFilm film)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"http://localhost:5005/api/FilmsGenres/createFilm", film);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                var MovieAddData = JsonSerializer.Deserialize<Film>(responseContent);

                return MovieAddData ?? new Film();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запросе: {ex.Message}");
                return new Film();
            }
        }

        public async void EditFilmAsync(Film film)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync("http://localhost:5005/api/FilmsGenres/editFilm", film);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе");
            }
        }

        public async void DeleteFilmAsync(int Id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:5005/api/FilmsGenres/deleteFilm/{Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе");
            }
        }
    }
}

