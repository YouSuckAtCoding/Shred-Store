using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShredStore.Models;
using ShredStore.Services;

namespace ShredStore.Controllers
{
    public class ShredStoreController : Controller
    {
        private readonly IProductHttpService product;
        private readonly IUserHttpService user;
        public const string SessionKeyName = "_Name";
        public const string SessionKeyId = "_Id";

        public ShredStoreController(IProductHttpService _product, IUserHttpService _user)
        {

            product = _product;
            user = _user;
        }        

        // GET: ShredStoreController
        public async Task<IActionResult> Index()
        {
            
            var products = await product.GetAll();
            return View(products);
        }
        public async Task<IActionResult> Category(string Category)
        {
            var products = await product.GetAllByCategory(Category);
            ViewBag.Title = Category;
            return View(products);
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin)
        {
            8if (ModelState.IsValid)
            {
                var loggedUser = await user.Login(userLogin);
                if(loggedUser != null)
                {
                    if(loggedUser.Id != 0)
                    {
                        HttpContext.Session.SetString(SessionKeyName, loggedUser.Name);
                        HttpContext.Session.SetInt32(SessionKeyId, loggedUser.Id);
                        return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.SetString(SessionKeyName, "");
            HttpContext.Session.SetInt32(SessionKeyId, 0);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> UserDetails(int Id)
        {
            var selected = await user.GetById(Id);
            return View(selected);
        }

        // GET: ShredStoreController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ShredStoreController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShredStoreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShredStoreController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShredStoreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShredStoreController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShredStoreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
