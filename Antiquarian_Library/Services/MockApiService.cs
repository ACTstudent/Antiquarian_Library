using Antiquarian_Library.Models;
using System.Text.Json;

namespace Antiquarian_Library.Services
{
    public class MockApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public MockApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            // The clone URL from MockAPI is 69d246015043d95be971ae8f
            // We use a public dummy MockAPI endpoint or the clone base
            _baseUrl = config["MockApiUrl"] ?? "https://6602eef59d7276a755546ad2.mockapi.io/api/v1/";
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        // --- Books ---
        public async Task<List<Book>> GetBooksAsync()
        {
            var response = await _httpClient.GetAsync("Book");
            if (!response.IsSuccessStatusCode) return new List<Book>();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Book>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Book>();
        }

        public async Task<Book?> GetBookAsync(string id)
        {
            var response = await _httpClient.GetAsync($"Book/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Book>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task CreateBookAsync(Book book)
        {
            await _httpClient.PostAsJsonAsync("Book", book);
        }

        public async Task UpdateBookAsync(string id, Book book)
        {
            await _httpClient.PutAsJsonAsync($"Book/{id}", book);
        }

        public async Task DeleteBookAsync(string id)
        {
            await _httpClient.DeleteAsync($"Book/{id}");
        }

        // --- Users ---
        public async Task<List<User>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("Users");
            if (!response.IsSuccessStatusCode) return new List<User>();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<User>();
        }

        public async Task<User?> GetUserAsync(string id)
        {
            var response = await _httpClient.GetAsync($"Users/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task CreateUserAsync(User user)
        {
            await _httpClient.PostAsJsonAsync("Users", user);
        }

        public async Task UpdateUserAsync(string id, User user)
        {
            await _httpClient.PutAsJsonAsync($"Users/{id}", user);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _httpClient.DeleteAsync($"Users/{id}");
        }
    }
}
