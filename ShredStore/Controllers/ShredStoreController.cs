using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStore.Models;
using ShredStore.Services;
using ShredStore.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using ShredStore.Models.Utility;

namespace ShredStore.Controllers
{
    public class ShredStoreController : Controller
    {
        private readonly IProductHttpService product;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IDistributedCache cache;
        private readonly MiscellaneousUtilityClass utilityClass;
        public ShredStoreController(IProductHttpService _product, IWebHostEnvironment _hostEnvironment,
            IDistributedCache _cache, MiscellaneousUtilityClass utilityClass)
        {
            product = _product;
            hostEnvironment = _hostEnvironment;
            cache = _cache;
            this.utilityClass = utilityClass;
        }        

        // GET: ShredStoreController
        public async Task<IActionResult> Index(string Search = "")
        {
            string recordKey = "Products_";
            var products = await cache.GetRecordAsync<IEnumerable<ProductViewModel>>(recordKey);
            if(products is null)
            {
                try
                {
                    var getProducts = await product.GetAll();
                    await cache.SetRecordAsync(recordKey, getProducts, TimeSpan.FromSeconds(35));
                    if(Search != "")
                    {
                        if(Search != null)
                        {
                            var list = getProducts.Where(p => p.Name.Contains(Search) || p.Category.Contains(Search) || p.Brand.Contains(Search))
                            .OrderBy(p => p.Name);
                            ViewBag.Search = "Ok";
                            return View(list);
                        }
                    }
                    return View(getProducts);
                }
                catch (Exception ex)
                {
                    utilityClass.GetLog().Error(ex, "Exception caught at Index action in ShredStoreController.");
                }
                
            }
            if (Search != "")
            {
                if (Search != null)
                {
                    var list = products.Where(p => p.Name.Contains(Search) || p.Category.Contains(Search) || p.Brand.Contains(Search))
                    .OrderBy(p => p.Name);
                    ViewBag.Search = "Ok";
                    return View(list);
                }
            }
            return View(products);
        }
        public async Task<IActionResult> Category(string Category)
        {
            string recordKey = $"{Category}_";
            var products = await cache.GetRecordAsync<IEnumerable<ProductViewModel>>(recordKey);
            if(products == null)
            {
                try
                {
                    var getProducts = await product.GetAllByCategory(Category);
                    await cache.SetRecordAsync(recordKey, getProducts, TimeSpan.FromSeconds(35));
                    ViewBag.Title = Category;
                    return View(getProducts);
                }
                catch (Exception ex)
                {
                    utilityClass.GetLog().Error(ex, "Exception caught at Category action in ShredStoreController.");
                }
                
            }
            return View(products);

        }
        public async Task<IActionResult> EmptyCart()
        {
            ViewBag.NoProds = "True";
            ViewBag.Message = "No products in cart!";
            string recordKey = "Products_";
            try
            {
                var products = await cache.GetRecordAsync<IEnumerable<ProductViewModel>>(recordKey);
                if (products is null)
                {
                    var getProducts = await product.GetAll();
                    await cache.SetRecordAsync(recordKey, getProducts);
                    return View("Index", getProducts);
                }
                return View("Index", products);
            }
            catch (Exception ex)
            {
                utilityClass.GetLog().Error(ex, "Exception caught at EmptyCart action in ShredStoreController.");
            }
            return RedirectToAction(nameof(Index));
        }
      
        public async Task<IActionResult> ProductDetails(int Id)
        {
            try
            {
                var selected = await product.GetById(Id);
                return View(selected);
            }
            catch (Exception ex)
            {
                utilityClass.GetLog().Error(ex, "Exception caught at ProductDetails action in ShredStoreController.");
                return RedirectToAction(nameof(Index));
            }

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
                    newProduct.Brand = productInfo.Brand;
                    newProduct.Description = productInfo.Description;
                    newProduct.Category = productInfo.Category;
                    newProduct.UserId = productInfo.UserId;
                    newProduct.Price = productInfo.Price;
                    newProduct.ImageName = await UploadImage(productInfo.ImageFile);
                    await product.Create(newProduct);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    utilityClass.GetLog().Error(ex, "Exception caught at PublishProduct action in ShredStoreController.");
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
        public async Task<IActionResult> EditProduct(int id)
        {
            var selected = await product.GetById(id);
            var list = Categories();
            ViewBag.Categories = new SelectList(list);
            return View(selected);
        }

        // POST: ShredStoreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(ProductViewModel edited)
        {
            ModelState.Remove("ImageFile");
            if (ModelState.IsValid)
            {
                try
                {
                    ProductViewModel newProduct = new ProductViewModel();
                    newProduct.Id = edited.Id;
                    newProduct.Name = edited.Name;
                    newProduct.Brand = edited.Brand;
                    newProduct.Description = edited.Description;
                    newProduct.Category = edited.Category;
                    newProduct.UserId = edited.UserId;
                    newProduct.Price = edited.Price; 
                    if(edited.ImageFile != null)
                    {
                        string res = DeleteImage(edited.ImageName);
                        newProduct.ImageName = await UploadImage(edited.ImageFile);
                        newProduct.ImageFile = edited.ImageFile;
                    }
                    newProduct.ImageName = edited.ImageName;
                    await product.Edit(newProduct);
                    return RedirectToAction(nameof(Index));                    
                }
                catch(Exception ex)
                {
                    utilityClass.GetLog().Error(ex, "Exception caught at EditProduct action in ShredStoreController.");
                    return View();
                }
            }

            return RedirectToAction(nameof(EditProduct), edited.Id);
        }
        public string DeleteImage(string image)
        {
            string rootPath = hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath + "/Images/", image);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                try
                {
                    file.Delete();
                    return "Ok";
                }
                catch (Exception ex)
                {
                    utilityClass.GetLog().Error(ex, "Exception caught at DeleteImage action in ShredStoreController.");
                    return "Error";
                }
                
            }
            else
            {
                return "Error";
            }
        }

        // GET: ShredStoreController/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var selected = await product.GetById(id);
            return View(selected);
        }

        // POST: ShredStoreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(ProductViewModel model)
        {
            int id = model.Id;
            try
            {

                var selected = await product.GetById(id);
                string result = DeleteImage(selected.ImageName);
                if(result == "Ok")
                {
                    await product.Delete(selected.Id);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Message = "A error occurred while deleting your product.";
                    return View();
                }
                   
            }
            catch(Exception ex)
            {
                utilityClass.GetLog().Error(ex, "Exception caught at DeleteProduct action in ShredStoreController.");
                return View();
            }
        }
        public async Task<IActionResult> AboutUs() => View();
    }
}
