using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStore.Factory.Interface;
using ShredStore.Models;
using ShredStore.Models.Utility;
using ShredStore.Services;
using System.Collections.Generic;

namespace ShredStore.Controllers
{
    public class CartOperations : Controller
    {
        private readonly ICartHttpService _cart;
        private readonly ICartItemHttpService _cartItem;
        private readonly IProductHttpService _product;
        private readonly MiscellaneousUtilityClass _utilityClass;
        private readonly ICartFactory _cartFactory;
        private readonly ICartItemFactory _cartItemFactory;
        private readonly int userId;

        public CartOperations(ICartHttpService _cart, IProductHttpService _product, ICartItemHttpService _cartItem, IHttpContextAccessor httpContext,
            MiscellaneousUtilityClass _utilityClass, ICartFactory _cartFactory, ICartItemFactory _cartItemFactory)
        {
            this._cart = _cart;
            this._product = _product;
            this._cartItem = _cartItem;
            userId = httpContext.HttpContext.Session.GetInt32("_Id").Value;
            this._utilityClass = _utilityClass;
            this._cartFactory = _cartFactory;
            this._cartItemFactory = _cartItemFactory;

        }
        private async void ValidateCart(CartViewModel selectedCart)
        {
            if ((DateTime.Now.Date - selectedCart.CreatedDate.Date).Days > 2)
            {
                await _cartItem.DeleteAll(selectedCart.Id);
                await _cart.Delete(selectedCart.Id);
                CreateCart();
                RedirectToAction(nameof(GetCart));

            }
        }
        private async Task<List<ProductViewModel>> SetCartListing(IEnumerable<CartItemViewModel> cartItems)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            foreach (var item in cartItems)
            {
                var prod = await _product.GetById(item.ProductId);
                products.Add(prod);

            }
            return products;
        }
        private Tuple<List<string>, decimal> SetListingAndPricingInfo(List<ProductViewModel> products)
        {
            decimal totalPrice = 0;
            List<string> productNames = new List<string>();
            foreach (var prod in products)
            {
                totalPrice = totalPrice + prod.Price;
                productNames.Add(prod.Name);
            }
            return new Tuple<List<string>, decimal>(productNames, totalPrice);
        }
        private List<ProductViewModel> RemoveDupesFromList(List<ProductViewModel> products)
        {
            List<ProductViewModel> noDupes = products.Distinct(new ProductComparer()).ToList();
            return noDupes;
        }
        public async Task<IActionResult> GetCart()
        {  
            var selectedCart = await _cart.GetById(userId);
            if (selectedCart != null)
            {
                try
                {
                    var cartItems = await _cartItem.GetAll(selectedCart.Id);
                    if (cartItems.Count() == 0)
                    {
                        return RedirectToAction("EmptyCart", "ShredStore");
                    }
                    ValidateCart(selectedCart);
                    var products = await SetCartListing(cartItems);
                    var infoTuple = SetListingAndPricingInfo(products);
                    var noDupes = RemoveDupesFromList(products);
                    ViewBag.ProductNames = _utilityClass.SetProductList(infoTuple.Item1);
                    ViewBag.TotalPrice = float.Parse(infoTuple.Item2.ToString());
                    return View(noDupes);
                }
                catch (Exception ex)
                {
                    _utilityClass.GetLog().Error(ex, "Exception caught at GetCart action in CartOperationsController.");
                }
            }
            return RedirectToAction("Index", "ShredStore");
        }
        public async void CreateCart()
        {
            if(userId != 0)
            {
                try
                {
                    var newCart = _cartFactory.CreateCart(userId);
                    await _cart.Create(newCart);
                }
                catch (Exception ex)
                {
                    _utilityClass.GetLog().Error(ex, "Exception caught at CreateCart action in CartOperationsController.");
                }
            }
            RedirectToAction("NoAccount", "UserOperations");
        }
        public async Task<IActionResult> InsertCart(int productId)
        {
            if(userId == 0)
            {
                return RedirectToAction("NoAccount", "UserOperations");
            }
            try
            {
                var shopcart = await _cart.GetById(userId);
                if (shopcart.Id == 0)
                {
                    CreateCart();
                    return RedirectToAction("InsertCart");
                }
                var cartItemViewModel = _cartItemFactory.CreateCartItem(shopcart, productId);
                await _cartItem.Create(cartItemViewModel);
            }
            catch (Exception ex)
            {
                _utilityClass.GetLog().Error(ex, "Exception caught at InsertCart action in CartOperationsController.");
            }
            return RedirectToAction("Index", "ShredStore");
        }
        public async Task<IActionResult> RemoveCartItem(int productId, int amount)
        {
            try
            {
                var shopcart = await _cart.GetById(userId);
                await _cartItem.Delete(productId, amount, shopcart.Id);
            }
            catch (Exception ex)
            {
                _utilityClass.GetLog().Error(ex, "Exception caught at RemoveCartItem action in CartOperationsController.");
            }
            
            
            return RedirectToAction(nameof(GetCart));
        }
    }
}
