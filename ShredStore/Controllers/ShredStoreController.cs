using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStore.Models;
using ShredStore.Services;
using ShredStore.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using ShredStore.Models.Utility;
using ShredStore.Factory.Interface;

namespace ShredStore.Controllers
{
    public class ShredStoreController : Controller
    {
        private readonly IProductHttpService _product;
        private readonly IDistributedCache _cache;
        private readonly MiscellaneousUtilityClass _utilityClass;
        private readonly IProductFactory _productFactory;
        public ShredStoreController(IProductHttpService _product, 
            IDistributedCache _cache, MiscellaneousUtilityClass _utilityClass, IProductFactory _productFactory)
        {
            this._product = _product;
            this._cache = _cache;
            this._utilityClass = _utilityClass;
            this._productFactory = _productFactory;
        }
        public async Task<IEnumerable<ProductViewModel>> GetAllProducts(string recordKey)
        {
            var products = await _cache.GetRecordAsync<IEnumerable<ProductViewModel>>(recordKey);
            if (products is null)
            {
                var getProducts = await _product.GetAll();
                SetOnCache(recordKey, getProducts);
                return getProducts;
            }
            return products;
        }
        public async Task<IEnumerable<ProductViewModel>> GetCategoryProducts(string recordKey, string category)
        {
            var products = await _cache.GetRecordAsync<IEnumerable<ProductViewModel>>(recordKey);
            if (products is null)
            {
                var getProducts = await _product.GetAllByCategory(category);
                SetOnCache(recordKey, getProducts);
                return getProducts;
            }
            return products;
        }
        public IEnumerable<ProductViewModel> SearchResults(string search, IEnumerable<ProductViewModel> products)
        {
            var searchResults = products.Where(p => p.Name.Contains(search) || p.Category.Contains(search) || p.Brand.Contains(search))
                         .OrderBy(p => p.Name);
            return searchResults;
        }
        public async void SetOnCache(string recordKey, IEnumerable<ProductViewModel> products)
        {
            await _cache.SetRecordAsync(recordKey, products, TimeSpan.FromSeconds(35));
        }
        
        
        // GET: ShredStoreController
        public async Task<IActionResult> Index(string Search = "")
        {
            string recordKey = "Products_";
            var allProducts = await GetAllProducts(recordKey);
            try
            {
                if (Search == "" || Search is null)
                {
                    return View(allProducts);
                }
                else
                {
                    var list = SearchResults(Search, allProducts);
                    ViewBag.Search = "Ok";
                    return View(list);
                }
            }
            catch (Exception ex)
            {
                _utilityClass.GetLog().Error(ex, "Exception caught at Index action in ShredStoreController.");
            }
            return View();
        }
        public async Task<IActionResult> Category(string Category)
        {
            string recordKey = $"{Category}_";
            var products = await GetCategoryProducts(recordKey, Category);
            try
            {
                ViewBag.Title = Category;
                return View(products);
            }
            catch (Exception ex)
            {
                _utilityClass.GetLog().Error(ex, "Exception caught at Category action in ShredStoreController.");
            } 
            return View();

        }
        public async Task<IActionResult> EmptyCart()
        {
            ViewBag.NoProds = "True";
            ViewBag.Message = "No products in cart!";
            string recordKey = "Products_";
            try
            {
                var products = await GetAllProducts(recordKey);                
                return View("Index", products);
            }
            catch (Exception ex)
            {
                _utilityClass.GetLog().Error(ex, "Exception caught at EmptyCart action in ShredStoreController.");
            }
            return RedirectToAction(nameof(Index));
        }
      
        public async Task<IActionResult> ProductDetails(int Id)
        {
            try
            {
                var selected = await _product.GetById(Id);
                return View(selected);
            }
            catch (Exception ex)
            {
                _utilityClass.GetLog().Error(ex, "Exception caught at ProductDetails action in ShredStoreController.");
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
                    ProductViewModel newProduct = await _productFactory.createProduct(productInfo);                    
                    await _product.Create(newProduct);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    _utilityClass.GetLog().Error(ex, "Exception caught at PublishProduct action in ShredStoreController.");
                    return View();
                }
                
            }
            
            return View();
        }
        // GET: ShredStoreController/Edit/5
        public async Task<IActionResult> EditProduct(int id)
        {
            var selected = await _product.GetById(id);
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
                    ProductViewModel newProduct = await _productFactory.createProduct(edited);
                    await _product.Edit(newProduct);
                    return RedirectToAction(nameof(Index));                    
                }
                catch(Exception ex)
                {
                    _utilityClass.GetLog().Error(ex, "Exception caught at EditProduct action in ShredStoreController.");
                    return View();
                }
            }

            return RedirectToAction(nameof(EditProduct), edited.Id);
        }
       

        // GET: ShredStoreController/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var selected = await _product.GetById(id);
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

                var selected = await _product.GetById(id);
                string result = _utilityClass.DeleteImage(selected.ImageName);
                if(result == "Ok")
                {
                    await _product.Delete(selected.Id);
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
                _utilityClass.GetLog().Error(ex, "Exception caught at DeleteProduct action in ShredStoreController.");
                return View();
            }
        }
        public async Task<IActionResult> AboutUs() => View();
    }
}
