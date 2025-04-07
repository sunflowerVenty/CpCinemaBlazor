﻿using Blazored.LocalStorage;
using CpCinemaBlazor.ApiRequest.Model;
using static CpCinemaBlazor.Components.Pages.ChatPage;

namespace CpCinemaBlazor.ApiRequest.Services
{
    public class MessagesService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly NotificationService _notificationService;
        private readonly ILocalStorageService _localStorage;
        private const string BaseUrl = "api/messages";

        public MessagesService(IHttpClientFactory httpClientFactory, NotificationService notificationService, ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _notificationService = notificationService;
            _localStorage = localStorage;
        }

        public async Task<List<MessagesDto>> getMessagesWithUserAsync(int userId, int senderId)
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");
            var url = $"{BaseUrl}/getMessagesWithUser/{userId}/{senderId}";
            return await client.GetFromJsonAsync<List<MessagesDto>>(url);
        }

        public async Task<List<UserMessageDto>> getMessagesAsync(int userId)
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");
            var url = $"{BaseUrl}/getMessages/{userId}";
            return await client.GetFromJsonAsync<List<UserMessageDto>>(url);
        }

        public async Task<List<MessagesFilm>> getMessagesFilmAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AuthorizedClient");
                var url = $"{BaseUrl}/getMessagesFilms/{id}";
                return await client.GetFromJsonAsync<List<MessagesFilm>>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении сообщений: {ex.Message}");
                return new List<MessagesFilm>();
            }
        }
    }
}
