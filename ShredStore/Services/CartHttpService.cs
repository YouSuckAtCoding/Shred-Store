using ShredStore.Models;
using System.Text.Json;

namespace ShredStore.Services
{
    public class CartHttpService : ICartHttpService
    {
        private readonly HttpClient httpClient;

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true

        };

        public CartHttpService(IConfiguration config)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(config.GetValue<string>("ApiUri"));
        }

        public async Task<CartViewModel> Create(CartViewModel cart)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"api/v1/Cart", cart);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var created = await JsonSerializer.DeserializeAsync<CartViewModel>(contentStream, jsonSerializerOptions);

            return created;

        }
        public async Task Delete(int id)
        {
            var httpResponseMessage = await httpClient.DeleteAsync($"api/v1/Cart/{id}");
            httpResponseMessage.EnsureSuccessStatusCode();

        }
        public async Task<CartViewModel> GetById(int id)
        {
            var cart = await httpClient.GetFromJsonAsync<CartViewModel>($"api/v1/Cart/{id}");
            return cart;
        }

    }
}
