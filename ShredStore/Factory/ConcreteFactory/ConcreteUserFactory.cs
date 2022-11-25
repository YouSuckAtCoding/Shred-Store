using ShredStore.Factory.Interface;
using ShredStore.Models;
using ShredStore.Models.Utility;

namespace ShredStore.Factory.ConcreteFactory
{
    public class ConcreteUserFactory : IUserFactory
    {
        private readonly ListCorrector _listCorrector;
        private readonly MiscellaneousUtilityClass _utilityClass;
        public ConcreteUserFactory(ListCorrector _listCorrector, MiscellaneousUtilityClass utilityClass)
        {
            this._listCorrector = _listCorrector;
            _utilityClass = utilityClass;
        }
        public UserRegistrationViewModel CreateUser(UserRegistrationViewModel user)
        {
            UserRegistrationViewModel newUser = new UserRegistrationViewModel();
            newUser.Name = user.Name;

            if (_listCorrector.IsEmailValid(user.Email))
            {
                newUser.Email = user.Email;
            }
            else
            {
                return new UserRegistrationViewModel();
            }
            newUser.Password = user.Password;
            if (user.Role == "Shop")
            {
                newUser.Role = "Vendedor";
            }
            else
            {
                newUser.Role = "Comprador";
            }

            return newUser;
        }

        public UserViewModel CreateUser(UserViewModel user)
        { 
            UserViewModel newUser = new UserViewModel();
            newUser.Id = user.Id;
            newUser.Name = user.Name;
            if (_listCorrector.IsEmailValid(user.Email))
            {
                newUser.Email = user.Email;
            }
            else
            {
                return new UserViewModel();
            }
            if (user.Role == "Shop")
            {
                newUser.Role = "Vendedor";
            }
            else
            {
                newUser.Role = "Comprador";
            }

            return newUser;
        }

        public UserResetPasswordViewModel CreateUser(string Email)
        {
            UserResetPasswordViewModel randomUser = new UserResetPasswordViewModel();
            randomUser.Email = Email;
            randomUser.Password = _utilityClass.CreateRandomPassword(10);
            return randomUser;
        }
    }
}
