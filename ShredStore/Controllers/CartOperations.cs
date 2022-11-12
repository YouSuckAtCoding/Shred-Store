using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShredStore.Models;
using ShredStore.Models.Utility;
using ShredStore.Services;

namespace ShredStore.Controllers
{
    public class CartOperations : Controller
    {
        private readonly ICartHttpService cart;
        private readonly ICartItemHttpService cartItem;
        private readonly IProductHttpService product;
        private readonly ListCorrector listCorrector;
        private readonly int userId;

        public CartOperations(ICartHttpService _cart, IProductHttpService _product, ICartItemHttpService _cartItem, IHttpContextAccessor httpContext,
            ListCorrector _listCorrector)
        {
            cart = _cart;
            product = _product;
            cartItem = _cartItem;
            userId = httpContext.HttpContext.Session.GetInt32("_Id").Value;
            listCorrector = _listCorrector;

        }
        public async Task<IActionResult> GetCart()
        {  
            var selectedCart = await cart.GetById(userId);
            if (selectedCart != null)
            {
                var cartItems = await cartItem.GetAll(selectedCart.Id);
                if (cartItems.Count() == 0)
                {
                    return RedirectToAction("EmptyCart", "ShredStore");
                }

                if ((DateTime.Now.Date - selectedCart.CreatedDate.Date).Days > 2)
                {
                    await cartItem.DeleteAll(selectedCart.Id);
                    await cart.Delete(selectedCart.Id);
                    CreateCart();
                    return RedirectToAction(nameof(GetCart));

                }
               
                List<ProductViewModel> products = new List<ProductViewModel>();
                decimal totalPrice = 0;
                List<string> productNames = new List<string>();
               
                foreach (var item in cartItems)
                {
                    var prod = await product.GetById(item.ProductId);
                    totalPrice = totalPrice + prod.Price;
                    productNames.Add(prod.Name);
                    products.Add(prod);

                }
                List<ProductViewModel> noDupes = products.Distinct(new ProductComparer()).ToList();
                ViewBag.ProductNames = listCorrector.SetProductList(productNames);
                ViewBag.TotalPrice = float.Parse(totalPrice.ToString());
                return View(noDupes);
            }
            return RedirectToAction("Index", "ShredStore");

        }
        public async void CreateCart()
        {
            if(userId == 0)
            {
                RedirectToAction("NoAccount", "UserOperations");
            }
            else
            {
                CartViewModel newCart = new CartViewModel();
                newCart.UserId = userId;
                newCart.CreatedDate = DateTime.Now;
                await cart.Create(newCart);

            }
        }
        public async Task<IActionResult> InsertCart(int productId)
        {
            if(userId == 0)
            {
                return RedirectToAction("NoAccount", "UserOperations");
            }

            var shopcart = await cart.GetById(userId);
            if(shopcart.Id != 0)
            {
                CartItemViewModel cartItemViewModel = new CartItemViewModel();
                cartItemViewModel.ProductId = productId;
                cartItemViewModel.CartId = shopcart.Id;
                await cartItem.Create(cartItemViewModel);
            }
            else
            {
                CreateCart();
                return RedirectToAction("InsertCart");
            }
            
            return RedirectToAction("Index", "ShredStore");
        }
        
        public async Task<IActionResult> RemoveCartItem(int productId, int amount)
        {
            var shopcart = await cart.GetById(userId);
            await cartItem.Delete(productId, amount, shopcart.Id);
            
            return RedirectToAction(nameof(GetCart));
        }
    }
}
