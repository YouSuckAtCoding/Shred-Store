using ShredStore.Models;

namespace ShredStore.Services
{
    public interface IUserHttpService
    {
        Task<UserRegistrationViewModel> Create(UserRegistrationViewModel user);
        Task Delete(int id);
        Task<UserViewModel> Edit(UserViewModel user);
        Task<IEnumerable<UserViewModel>> GetAll();
        Task<UserViewModel> GetById(int id);
        Task<UserViewModel> Login(UserLoginViewModel user);
        Task<bool> CheckEmail(string email);
        Task<bool> ResetUserPassword(UserResetPasswordViewModel user);
    }
}