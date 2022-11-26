﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStore.Factory.Interface;
using ShredStore.Models;
using ShredStore.Models.Utility;
using ShredStore.Services;

namespace ShredStore.Controllers
{
    public class UserOperationsController : Controller
    {
        private readonly IUserHttpService _user;
        private readonly IProductHttpService _product;
        private readonly EmailSender _emailSender;
        private readonly MiscellaneousUtilityClass _utilityClass;
        private readonly IUserFactory _userFactory;
        public const string SessionKeyName = "_Name";
        public const string SessionKeyId = "_Id";
        public const string SessionKeyRole = "_Role";
        public UserOperationsController(IUserHttpService _user, IProductHttpService _product,
            EmailSender _emailSender, MiscellaneousUtilityClass _utilityClass, IUserFactory _userFactory)
        {
            this._user = _user;
            this._product = _product;
            this._emailSender = _emailSender;
            this._utilityClass = _utilityClass;
            this._userFactory = _userFactory;
            
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
                var newUser = _userFactory.CreateUser(userData);
                try
                {
                    if(newUser.Id == 0)
                    {
                        ViewBag.Message = "Invalid Login";
                        return View();
                    }
                    await _user.Create(newUser);
                    await _emailSender.SendEmailAsync(newUser.Email, 2);
                    return RedirectToAction(nameof(Login));


                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error occurred while registering.";
                    _utilityClass.GetLog().Error(ex, "Exception caught at CreateAccount action in UserOperationsController.");
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
                var editedUser = _userFactory.CreateUser(userEdit);
                try
                {
                    if(editedUser.Id <= 0)
                    {
                        ViewBag.Message = "Something went wrong.";
                        return View();
                    }
                    await _user.Edit(editedUser);
                    return RedirectToAction("SetSessionInfo", "ShredStore", editedUser);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error has occurred.";
                    _utilityClass.GetLog().Error(ex, "Exception caught at EditAccount action in UserOperationsController.");
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
            if (_utilityClass.IsEmailValid(Email))
            {
                bool res = await _user.CheckEmail(Email);
                if (res)
                {
                    try
                    {
                        var randomUser = _userFactory.CreateUser(Email);
                        bool ok = await _user.ResetUserPassword(randomUser);
                        if (!ok)
                        {
                            ViewBag.Message = "Invalid Email.";
                            return View();
                            
                        }
                        await _emailSender.SendEmailAsync(Email, 1, randomUser);
                        return RedirectToAction("Index", "ShredStore");
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "An error has occurred.";
                        _utilityClass.GetLog().Error(ex, "Exception caught at PasswordReset action in UserOperationsController.");
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
                    var loggedUser = await _user.Login(userLogin);
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
                    _utilityClass.GetLog().Error(ex, "Exception caught at ChangePassword action in UserOperationsController.");
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
                bool ok = await _user.ResetUserPassword(newPassword);
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
                _utilityClass.GetLog().Error(ex, "Exception caught at NewPassword action in UserOperationsController.");
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
                    var loggedUser = await _user.Login(userLogin);
                    if (loggedUser != null)
                    {
                        int sessionId = HttpContext.Session.GetInt32("_Id").Value;
                        if (loggedUser.Id == sessionId)
                        {
                            await _user.Delete(sessionId);
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
                _utilityClass.GetLog().Error(ex, "Exception caught at DeleteAccount action in UserOperationsController.");
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
                    var loggedUser = await _user.Login(userLogin);
                    if (loggedUser != null && loggedUser.Id != 0)
                    {
                        SetSessionInfo(loggedUser);
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
                    _utilityClass.GetLog().Error(ex, "Exception caught at Login action in UserOperationsController.");
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
        public ActionResult SetSessionInfo(UserViewModel user)
        {
            HttpContext.Session.SetInt32(SessionKeyId, user.Id);
            HttpContext.Session.SetString(SessionKeyName, user.Name);
            HttpContext.Session.SetString(SessionKeyRole, user.Role);
            return (RedirectToAction(nameof(Index),"ShredStore"));
        }
        public async Task<IEnumerable<ProductViewModel>> UserProducts()
        {
            var products = await _product.GetAllByUserId(HttpContext.Session.GetInt32("_Id").Value);
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
            var selected = await _user.GetById(Id);
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
