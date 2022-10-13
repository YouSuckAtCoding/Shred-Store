using ShredStore.Models;
using System.Text.Json;

namespace ShredStore.Services
{
    public class ProductHttpService : IProductHttpService
    {
        private readonly HttpClient httpClient;

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true

        };

        public ProductHttpService(IConfiguration config)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(config.GetValue<string>("ApiUri"));
        }

        public async Task<ProductViewModel> Create(ProductViewModel product)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"api/v1/Product", product);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var created = await JsonSerializer.DeserializeAsync<ProductViewModel>(contentStream, jsonSerializerOptions);

            return created;

        }

        public async Task Delete(int id)
        {
            var httpResponseMessage = await httpClient.DeleteAsync($"api/v1/Product/{id}");
            httpResponseMessage.EnsureSuccessStatusCode();

        }
        public async Task<ProductViewModel> Edit(ProductViewModel product)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync($"api/v1/UsuarioApi/{product.Id}", product);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var edited = await JsonSerializer.DeserializeAsync<ProductViewModel>(contentStream, jsonSerializerOptions);
            return edited;
        }
        public async Task<IEnumerable<ProductViewModel>> GetAll()
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductViewModel>>($"api/v1/Product");
            return products;

        }
        public async Task<IEnumerable<ProductViewModel>> GetAllByCategory(string Category)
        {
            var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductViewModel>>($"api/v1/Product/GetAll/{Category}");
            return products;

        }
        public async Task<ProductViewModel> GetById(int id)
        {
            var Usuario = await httpClient.GetFromJsonAsync<ProductViewModel>($"api/v1/Product/{id}");
            return Usuario;
        }
    }
}

