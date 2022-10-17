using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStore.Models;
using ShredStore.Services;

namespace ShredStore.Controllers
{
    public class UserOperationsController : Controller
    {
        private readonly IUserHttpService user;

        public UserOperationsController(IUserHttpService _user)
        {
            user = _user;
        }
        [HttpGet]
        public async Task<IActionResult> CreateAccount()
        {
            List<string> Role = new List<string>(); 
            Role.Add("Shop");
            Role.Add("Customer");
            ViewBag.Roles = new SelectList(Role);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccount(UserRegistrationViewModel userData)
        {
            if (ModelState.IsValid)
            {
                UserRegistrationViewModel newUser = new UserRegistrationViewModel();
                newUser.Name = userData.Name;
                newUser.Email = userData.Email;
                newUser.Password = userData.Password;
                if(userData.Role == "Shop")
                {
                    newUser.Role = "Vendedor";
                }
                else
                {
                    newUser.Role = "Comprador";
                }
                await user.Create(newUser);
                return RedirectToAction("Login", "ShredStore");

            }
            return View();
        }
    }
}
