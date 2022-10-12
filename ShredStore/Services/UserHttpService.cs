using ShredStore.Models;
using System.Text.Json;

namespace ShredStore.Services
{
    public class UserHttpService : IUserHttpService
    {
        private readonly HttpClient httpClient;

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            IgnoreNullValues = true

        };

        public UserHttpService(IConfiguration config)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(config.GetValue<string>("ApiUri"));
        }
        public async Task<UserViewModel> Login(UserLoginViewModel user)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"api/v1/User/Login", user);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var created = await JsonSerializer.DeserializeAsync<UserViewModel>(contentStream, jsonSerializerOptions);

            return created;


        }
        public async Task<UserRegistrationViewModel> Create(UserRegistrationViewModel user)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"api/v1/User", user);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var created = await JsonSerializer.DeserializeAsync<UserRegistrationViewModel>(contentStream, jsonSerializerOptions);

            return created;

        }

        public async Task Delete(int id)
        {
            var httpResponseMessage = await httpClient.DeleteAsync($"api/v1/User/{id}");
            httpResponseMessage.EnsureSuccessStatusCode();

        }
        public async Task<UserViewModel> Edit(UserViewModel user)
        {
            var httpResponseMessage = await httpClient.PutAsJsonAsync($"api/v1/UsuarioApi/{user.Id}", user);

            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var edited = await JsonSerializer.DeserializeAsync<UserViewModel>(contentStream, jsonSerializerOptions);
            return edited;
        }
        public async Task<IEnumerable<UserViewModel>> GetAll()
        {
            var users = await httpClient.GetFromJsonAsync<IEnumerable<UserViewModel>>($"api/v1/User");
            return users;

        }
        public async Task<UserViewModel> GetById(int id)
        {
            var Usuario = await httpClient.GetFromJsonAsync<UserViewModel>($"api/v1/User/{id}");
            return Usuario;
        }
    }
}
