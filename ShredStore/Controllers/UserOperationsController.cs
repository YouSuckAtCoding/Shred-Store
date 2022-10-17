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
            var list = GetRoles();
            ViewBag.Roles = new SelectList(list);
            return View();
        }
        public List<string> GetRoles(){
            List<string> Role = new List<string>();
            Role.Add("Shop");
            Role.Add("Customer");
            return Role;
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
        [HttpGet]
        public async Task<IActionResult> EditAccount()
        {
            var list = GetRoles();
            ViewBag.Roles = new SelectList(list);
            return View();
        }
        [HttpPost]
        //public async Task<IActionResult> EditAccount(UserViewModel userEdit)
        //{
            
        //}
        [HttpGet]
        public async Task<IActionResult> DeleteAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(UserLoginViewModel userLogin)
        {
            userLogin.Name = HttpContext.Session.GetString("_Name");
            if (userLogin.Name != null && userLogin.Password != null)
            {
                var loggedUser = await user.Login(userLogin);
                if (loggedUser != null)
                {
                    int sessionId = HttpContext.Session.GetInt32("_Id").Value;
                    if(loggedUser.Id == sessionId)
                    {
                        await user.Delete(sessionId);                        
                        return RedirectToAction("Logout", "ShredStore");
                    }
                    
                }
                else
                {
                    return View();
                }
            }
            return View();
        }

    }
}
