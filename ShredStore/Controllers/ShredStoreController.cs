using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShredStore.Services;

namespace ShredStore.Controllers
{
    public class ShredStoreController : Controller
    {
        private readonly IProductHttpService product;

        public ShredStoreController(IProductHttpService _product)
        {
            product = _product;
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
