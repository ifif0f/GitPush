using System.Text.Json;
using WebGitPush.Models.News;
using WebGitPush.Models.Parking;
using WebGitPush.Models.Storages;

namespace WebGitPush.Classes
{
    public class ConnectionApi
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://hck-api.unicorn.icu/";
        private readonly string _token = "ust-2814999-6c079214805a94a50ecc1cb8ec40cc75";

        public ConnectionApi()
        {
            _client = new HttpClient();
            _client.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<ParkingApiResponse?> GetParkingDataAsync()
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}api/v1/parking/list?token={_token}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ParkingApiResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.error == 0 ? result : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения парковок: {ex.Message}");
                return null;
            }
        }

        public async Task<StorageApiResponse?> GetStorageDataAsync()
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}api/v1/storage/list?token={_token}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<StorageApiResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.error == 0 ? result : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения кладовок: {ex.Message}");
                return null;
            }
        }

        public async Task<NewsApiResponse?> GetNewsDataAsync()
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}api/v1/news/list?token={_token}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<NewsApiResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.error == 0 ? result : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения новостей: {ex.Message}");
                return null;
            }
        }
    }
}
