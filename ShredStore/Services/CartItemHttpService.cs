using ShredStore.Models;
using System.Text.Json;

namespace ShredStore.Services
{
    public class CartItemHttpService : ICartItemHttpService
    {
        private readonly HttpClient httpClient;

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true

        };

        public CartItemHttpService(IConfiguration config)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(config.GetValue<string>("ApiUri"));
        }

        public async Task<CartItemViewModel> Create(CartItemViewModel cartItem)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"api/v1/CartItem", cartItem);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var created = await JsonSerializer.DeserializeAsync<CartItemViewModel>(contentStream, jsonSerializerOptions);

            return created;

        }

        public async Task Delete(int id)
        {
            var httpResponseMessage = await httpClient.DeleteAsync($"api/v1/CartItem/{id}");
            httpResponseMessage.EnsureSuccessStatusCode();

        }
        public async Task<CartItemViewModel> Edit(CartItemViewModel cartItem)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync($"api/v1/UsuarioApi/{cartItem.Id}", cartItem);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var edited = await JsonSerializer.DeserializeAsync<CartItemViewModel>(contentStream, jsonSerializerOptions);
            return edited;
        }
        public async Task<IEnumerable<CartItemViewModel>> GetAll()
        {
            var cartItems = await httpClient.GetFromJsonAsync<IEnumerable<CartItemViewModel>>($"api/v1/CartItem");
            return cartItems;

        }
        public async Task<CartItemViewModel> GetById(int id)
        {
            var cartItem = await httpClient.GetFromJsonAsync<CartItemViewModel>($"api/v1/CartItem/{id}");
            return cartItem;
        }
    }
}
