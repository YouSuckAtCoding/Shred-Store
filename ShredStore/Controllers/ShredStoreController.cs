using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStore.Models;
using ShredStore.Services;

namespace ShredStore.Controllers
{
    public class ShredStoreController : Controller
    {
        private readonly IProductHttpService product;
        private readonly IWebHostEnvironment hostEnvironment;

        public ShredStoreController(IProductHttpService _product, IWebHostEnvironment _hostEnvironment)
        {
            product = _product;
            hostEnvironment = _hostEnvironment;
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
        public async Task<IActionResult> ProductDetails(int Id)
        {
            var selected = await product.GetById(Id);
            return View(selected);
        }
        public List<string> Categories()
        {
            List<string> categories = new List<string>();
            categories.Add("Eletric Guitar");
            categories.Add("Pedals");
            categories.Add("Amplifier");
            categories.Add("Accessories");
            categories.Add("Acoustic Guitar");
            return categories;
        }
       
        // GET: ShredStoreController/Create
        public IActionResult PublishProduct()
        {
            var list = Categories();
            ViewBag.Categories = new SelectList (list);
            return View();
        }

        // POST: ShredStoreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishProduct(ProductViewModel productInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProductViewModel newProduct = new ProductViewModel();
                    newProduct.Name = productInfo.Name;
                    newProduct.Description = productInfo.Description;
                    newProduct.Category = productInfo.Category;
                    newProduct.UserId = productInfo.UserId;
                    newProduct.Price = productInfo.Price;
                    newProduct.ImageName = await UploadImage(productInfo.ImageFile);
                    await product.Create(newProduct);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
                
            }
            return View();
        }
        public async Task<string> UploadImage(IFormFile image)
        {
            string rootPath = hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(image.FileName);
            string fileExtension = Path.GetExtension(image.FileName);
            string ImageName = fileName + fileExtension;
            string path = Path.Combine(rootPath + "/Images/", ImageName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return ImageName;
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
