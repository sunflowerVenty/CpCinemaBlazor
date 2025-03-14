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

        public async Task<List<Messages>> GetAllMessagesAsync()
        {
            var url = "api/Messages/GetAllMessages";

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

                var messages = JsonSerializer.Deserialize<List<Messages>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return messages ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе всех сообщений");
                return [];
            }
        }

        public async Task<Messages?> GetMessageByIdAsync(int id)
        {
            var url = $"api/Messages/GetMessageById/{id}";

            try
            {
                var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning("Ответ от сервера пуст.");
                    return null;
                }

                var message = JsonSerializer.Deserialize<Messages>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе сообщения по ID");
                return null;
            }
        }

        public async Task<bool> CreateMessageAsync(Messages message)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Messages/CreateMessage", message);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Ошибка при создании сообщения: {errorContent}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании сообщения");
                return false;
            }
        }

        public async Task<bool> UpdateMessageAsync(Messages message)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/Messages/UpdateMessage", message);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Ошибка при обновлении сообщения: {errorContent}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении сообщения");
                return false;
            }
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Messages/DeleteMessage/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Ошибка при удалении сообщения: {errorContent}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении сообщения");
                return false;
            }
        }

        public async Task<List<Messages>> GetMessagesByFilmIdAsync(int filmId)
        {
            var url = $"api/Messages/GetMessagesByFilmId/{filmId}";

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

                var messages = JsonSerializer.Deserialize<List<Messages>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return messages ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе сообщений для фильма");
                return [];
            }
        }

        public async Task<List<Messages>> GetChatMessagesAsync(int userId, int recipientId)
        {
            var url = $"api/Messages/GetChatMessages?userId={userId}&recipientId={recipientId}";

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

                var messages = JsonSerializer.Deserialize<List<Messages>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return messages ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе чат-сообщений");
                return [];
            }
        }

        public async Task<List<Messages>> GetLatestMessagesForUserAsync(int userId)
        {
            var url = $"api/Messages/GetLatestMessagesForUser/{userId}";

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

                var messages = JsonSerializer.Deserialize<List<Messages>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return messages ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе последних сообщений пользователя");
                return [];
            }
        }
    }
}

