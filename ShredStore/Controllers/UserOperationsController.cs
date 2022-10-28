using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStore.Models;
using ShredStore.Services;

namespace ShredStore.Controllers
{
    public class UserOperationsController : Controller
    {
        private readonly IUserHttpService user;
        private readonly IProductHttpService product;
        public const string SessionKeyName = "_Name";
        public const string SessionKeyId = "_Id";
        public const string SessionKeyRole = "_Role";

        public UserOperationsController(IUserHttpService _user, IProductHttpService _product)
        {
            user = _user;
            product = _product;
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
                return RedirectToAction(nameof(Login));

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
                newUser.Email = userEdit.Email;
                if (userEdit.Role == "Shop")
                {
                    newUser.Role = "Vendedor";
                }
                else
                {
                    newUser.Role = "Comprador";
                }
                await user.Edit(newUser);

                return RedirectToAction("ResetInfo", "ShredStore", newUser);

            }
            return View();
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
                var loggedUser = await user.Login(userLogin);
                if (loggedUser != null)
                {
                    if (loggedUser.Id != 0)
                    {
                        HttpContext.Session.SetString(SessionKeyName, loggedUser.Name);
                        HttpContext.Session.SetInt32(SessionKeyId, loggedUser.Id);
                        HttpContext.Session.SetString(SessionKeyRole, loggedUser.Role);
                        return RedirectToAction(nameof(Index),"ShredStore");
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
            return View(selected);
        }

    }
}
