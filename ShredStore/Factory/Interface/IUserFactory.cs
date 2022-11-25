using ShredStore.Models;

namespace ShredStore.Factory.Interface
{
    public interface IUserFactory
    {
        UserRegistrationViewModel CreateUser(UserRegistrationViewModel user);

        UserViewModel CreateUser(UserViewModel user);

        UserResetPasswordViewModel CreateUser(string Email);
    }
}
