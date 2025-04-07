using Blazored.LocalStorage;
using CpCinemaBlazor.ApiRequest.Model;
using System.Net.Http.Headers;

namespace CpCinemaBlazor.ApiRequest.Services
{
    public class FilmService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly NotificationService _notificationService;
        private readonly ILocalStorageService _localStorage;
        private const string BaseUrl = "api/Films";

        public FilmService(IHttpClientFactory httpClientFactory, NotificationService notificationService, ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _notificationService = notificationService;
            _localStorage = localStorage;

        }

        public async Task<List<Film>> GetFilmsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AuthorizedClient");
                var url = $"{BaseUrl}/GetAllFilms";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _notificationService.ShowError($"Ошибка при получении фильмов. Код: {response.StatusCode}");
                    return new List<Film>();
                }

                var result = await response.Content.ReadFromJsonAsync<List<Film>>();
                return result ?? new List<Film>();
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Ошибка при получении списка фильмов: {ex.Message}");
                return new List<Film>();
            }
        }

        public async Task<Film?> GetFilmAsync(int? id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AuthorizedClient");
                return await client.GetFromJsonAsync<Film>($"{BaseUrl}/GetFilm/{id}");
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Ошибка при получении фильма: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateFilmAsync(Film Film)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AuthorizedClient");
                var token = await _localStorage.GetItemAsync<string>("authToken");

                if (string.IsNullOrEmpty(token))
                {
                    _notificationService.ShowError("Не найден токен авторизации");
                    return false;
                }

                // Создаем запрос вручную
                var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/CreateFilm");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = JsonContent.Create(Film);

                var response = await client.SendAsync(request);

                // Проверяем статус ответа
                if (response.IsSuccessStatusCode)
                {
                    _notificationService.ShowSuccess("Фильм успешно добавлен");
                    return true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _notificationService.ShowError("Ошибка авторизации. Пожалуйста, войдите снова");
                    return false;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _notificationService.ShowError($"Ошибка при создании фильма. Статус: {response.StatusCode}. {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Ошибка при создании фильма: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateFilmAsync(int id, Film Film)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AuthorizedClient");
                var token = await _localStorage.GetItemAsync<string>("authToken");

                if (string.IsNullOrEmpty(token))
                {
                    _notificationService.ShowError("Не найден токен авторизации");
                    return false;
                }

                // Создаем запрос вручную
                var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/UpdateFilm/{id}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = JsonContent.Create(Film);

                var response = await client.SendAsync(request);

                // Проверяем статус ответа
                if (response.IsSuccessStatusCode)
                {
                    _notificationService.ShowSuccess("Фильм успешно обновлен");
                    return true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _notificationService.ShowError("Ошибка авторизации. Пожалуйста, войдите снова");
                    return false;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _notificationService.ShowError("Фильм не найден");
                    return false;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _notificationService.ShowError($"Ошибка при обновлении фильма. Статус: {response.StatusCode}. {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Ошибка при обновлении фильма: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteFilmAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AuthorizedClient");
                var token = await _localStorage.GetItemAsync<string>("authToken");

                if (string.IsNullOrEmpty(token))
                {
                    _notificationService.ShowError("Не найден токен авторизации");
                    return false;
                }

                var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/DeleteFilm/{id}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _notificationService.ShowSuccess("Фильм успешно удален");
                    return true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _notificationService.ShowError("Ошибка авторизации. Пожалуйста, войдите снова");
                    return false;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _notificationService.ShowError("Фильм не найден");
                    return false;
                }
                else
                {
                    _notificationService.ShowError($"Ошибка при удалении фильма. Код: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Ошибка при удалении фильма: {ex.Message}");
                return false;
            }
        }
        public async Task<Film?> SearchFilmByIdAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AuthorizedClient");
                var token = await _localStorage.GetItemAsync<string>("authToken");

                if (string.IsNullOrEmpty(token))
                {
                    _notificationService.ShowError("Не найден токен авторизации");
                    return null;
                }

                var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/GetFilm/{id}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var Film = await response.Content.ReadFromJsonAsync<Film>();
                    if (Film != null)
                    {
                        return Film;
                    }
                    _notificationService.ShowError("Фильм не найден");
                    return null;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _notificationService.ShowError("Фильм не найден");
                    return null;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _notificationService.ShowError($"Ошибка при поиске фильма. Код: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _notificationService.ShowError($"Ошибка при поиске фильма: {ex.Message}");
                return null;
            }
        }
    }
}
