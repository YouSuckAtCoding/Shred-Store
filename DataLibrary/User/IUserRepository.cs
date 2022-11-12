using Models;

namespace DataLibrary.User
{
    public interface IUserRepository
    {
        Task DeleteUser(int id);
        Task<UserModel?> GetUser(int id);
        Task<UserModel?> Login(string Name, string Password);
        Task<IEnumerable<UserModel>> GetUsuarios();
        Task InsertUser(UserRegisterModel user);
        Task UpdateUser(UserModel user);
        Task<string> CheckUserEmail(string Email);
        Task<bool> ResetUserPassword(string NewPassword, string Email);
    }
}