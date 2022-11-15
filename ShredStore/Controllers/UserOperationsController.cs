using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStore.Models;
using ShredStore.Models.Utility;
using ShredStore.Services;

namespace ShredStore.Controllers
{
    public class UserOperationsController : Controller
    {
        private readonly IUserHttpService user;
        private readonly IProductHttpService product;
        private readonly ListCorrector listCorrector;
        private readonly EmailSender emailSender;
        private readonly MiscellaneousUtilityClass utilityClass;
        public const string SessionKeyName = "_Name";
        public const string SessionKeyId = "_Id";
        public const string SessionKeyRole = "_Role";
        public UserOperationsController(IUserHttpService _user, IProductHttpService _product, ListCorrector _listCorrector,
            EmailSender _emailSender, MiscellaneousUtilityClass utilityClass)
        {
            user = _user;
            product = _product;
            listCorrector = _listCorrector;
            emailSender = _emailSender;
            this.utilityClass = utilityClass;
            
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
                try
                {
                    UserRegistrationViewModel newUser = new UserRegistrationViewModel();
                    newUser.Name = userData.Name;

                    if (listCorrector.IsEmailValid(userData.Email))
                    {
                        newUser.Email = userData.Email;
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Email Format";
                        return View("Login");
                    }
                    newUser.Password = userData.Password;
                    if (userData.Role == "Shop")
                    {
                        newUser.Role = "Vendedor";
                    }
                    else
                    {
                        newUser.Role = "Comprador";
                    }
                    await user.Create(newUser);
                    await emailSender.SendEmailAsync(newUser.Email, 2);
                    return RedirectToAction(nameof(Login));
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error occurred while registering.";
                    utilityClass.GetLog().Error(ex, "Exception caught at CreateAccount action in UserOperationsController.");
                    return View();
                  
                }
                
                
                

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
        public async Task<IActionResult> EditAccount(UserViewModel userEdit)
        {
            if (ModelState.IsValid)
            {
                UserViewModel newUser = new UserViewModel();
                newUser.Id = userEdit.Id;
                newUser.Name = userEdit.Name;
                if (listCorrector.IsEmailValid(userEdit.Email))
                {
                    newUser.Email = userEdit.Email;
                }
                else
                {
                    ViewBag.Message = "Invalid Email Format";
                    return View("Login");
                }
                
                if (userEdit.Role == "Shop")
                {
                    newUser.Role = "Vendedor";
                }
                else
                {
                    newUser.Role = "Comprador";
                }
                try
                {
                    await user.Edit(newUser);
                    return RedirectToAction("ResetInfo", "ShredStore", newUser);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error has occurred.";
                    utilityClass.GetLog().Error(ex, "Exception caught at EditAccount action in UserOperationsController.");
                    return View();
                    
                }
                

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PasswordReset()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset(string Email)
        {
            if (listCorrector.IsEmailValid(Email))
            {
                bool res = await user.CheckEmail(Email);
                if (res)
                {
                    try
                    {
                        UserResetPasswordViewModel randomUser = new UserResetPasswordViewModel();
                        randomUser.Email = Email;
                        randomUser.Password = utilityClass.CreateRandomPassword(10);
                        bool ok = await user.ResetUserPassword(randomUser);
                        if (ok)
                        {
                            await emailSender.SendEmailAsync(Email, 1, randomUser);
                            return RedirectToAction("Index", "ShredStore");
                        }
                        else
                        {
                            ViewBag.Message = "Invalid Email.";
                            return View();
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "An error has occurred.";
                        utilityClass.GetLog().Error(ex, "Exception caught at PasswordReset action in UserOperationsController.");
                        return View();
                        throw;
                    }
                    
                }
                else
                {
                    ViewBag.Message = "Invalid Email.";
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserLoginViewModel userLogin)
        {
            userLogin.Name = HttpContext.Session.GetString("_Name");
            if (userLogin.Name != null && userLogin.Password != null)
            {
                try
                {
                    var loggedUser = await user.Login(userLogin);
                    if (loggedUser != null)
                    {
                        int sessionId = HttpContext.Session.GetInt32("_Id").Value;
                        if (loggedUser.Id == sessionId)
                        {
                            return RedirectToAction("NewPassword");
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error has occurred.";
                    utilityClass.GetLog().Error(ex, "Exception caught at ChangePassword action in UserOperationsController.");
                    return View();
                    throw;
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> NewPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewPassword(UserResetPasswordViewModel newPassword)
        {
            try
            {
                bool ok = await user.ResetUserPassword(newPassword);
                if (ok)
                {
                    return RedirectToAction("Index", "ShredStore");
                }
                else
                {
                    ViewBag.Message = "Invalid Email.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error has occurred.";
                utilityClass.GetLog().Error(ex, "Exception caught at NewPassword action in UserOperationsController.");
                return View();
            }
            
               
                
            
        }
        [HttpGet]
        public async Task<IActionResult> DeleteAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(UserLoginViewModel userLogin)
        {
            userLogin.Name = HttpContext.Session.GetString("_Name");
            try
            {
                if (userLogin.Name != null && userLogin.Password != null)
                {
                    var loggedUser = await user.Login(userLogin);
                    if (loggedUser != null)
                    {
                        int sessionId = HttpContext.Session.GetInt32("_Id").Value;
                        if (loggedUser.Id == sessionId)
                        {
                            await user.Delete(sessionId);
                            return RedirectToAction(nameof(Logout));
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {

                ViewBag.Message = "An error has occurred.";
                utilityClass.GetLog().Error(ex, "Exception caught at DeleteAccount action in UserOperationsController.");
                return View();
            }

            return View();
        }
        public IActionResult NoAccount()
        {
            ViewBag.Message = "Please create an account to add to cart.";
            return View("Login");
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var loggedUser = await user.Login(userLogin);
                    if (loggedUser != null)
                    {
                        if (loggedUser.Id != 0)
                        {
                            HttpContext.Session.SetString(SessionKeyName, loggedUser.Name);
                            HttpContext.Session.SetInt32(SessionKeyId, loggedUser.Id);
                            HttpContext.Session.SetString(SessionKeyRole, loggedUser.Role);
                            return RedirectToAction(nameof(Index), "ShredStore");
                        }
                        else
                        {
                            ViewBag.Message = "User does not exists!";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "User does not exists!";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error has occurred.";
                    utilityClass.GetLog().Error(ex, "Exception caught at Login action in UserOperationsController.");
                    return View();
                }
                
                

            }
            return View();

        }
        public ActionResult Logout()
        {
            HttpContext.Session.SetString(SessionKeyName, "");
            HttpContext.Session.SetInt32(SessionKeyId, 0);
            HttpContext.Session.SetString(SessionKeyRole, "");
            return RedirectToAction(nameof(Index), "ShredStore");
        }
        public ActionResult ResetInfo(UserViewModel user)
        {
            HttpContext.Session.SetString(SessionKeyName, user.Name);
            HttpContext.Session.SetString(SessionKeyRole, user.Role);
            return (RedirectToAction(nameof(Index),"ShredStore"));
        }
        public async Task<IEnumerable<ProductViewModel>> UserProducts()
        {
            var products = await product.GetAllByUserId(HttpContext.Session.GetInt32("_Id").Value);
            if (products != null)
            {
                return products;
            }
            return Enumerable.Empty<ProductViewModel>();
        }
        public async Task<IActionResult> UserDetails(int Id)
        {
            var userProducts = await UserProducts();
            ViewBag.UserProducts = userProducts;
            var selected = await user.GetById(Id);
            switch (selected.Role)
            {
                case "Comprador":
                    selected.Role = "Customer";
                    break;
                case "Vendedor":
                    selected.Role = "Shop";
                    break;
                default:
                    break;
            }
            return View(selected);
        }

    }
}
