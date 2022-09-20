using Models;

namespace DataLibrary.User
{
    public interface IUserRepository
    {
        Task DeleteUser(int id);
        Task<UserModel> GetUser(int id);
        Task<IEnumerable<UserModel>> GetUsuarios();
        Task InsertUser(UserRegisterModel user);
        Task UpdateUser(UserModel user);
    }
}