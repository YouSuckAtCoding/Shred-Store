using Microsoft.AspNetCore.Mvc;
using ShredStore.Models;
using ShredStore.Services;

namespace ShredStore.Controllers
{
    public class CartOperations : Controller
    {
        private readonly ICartHttpService cart;
        private readonly ICartItemHttpService cartItem;
        private readonly IProductHttpService product;
        private readonly int userId;

        public CartOperations(ICartHttpService _cart, IProductHttpService _product, ICartItemHttpService _cartItem, IHttpContextAccessor httpContext)
        {
            cart = _cart;
            product = _product;
            cartItem = _cartItem;
            userId = httpContext.HttpContext.Session.GetInt32("_Id").Value;

        }
        public async Task<IActionResult> GetCart()
        {
            
            var selected = await cart.GetById(userId);      
            return View(selected);
            
        }
        public async Task<IActionResult> InsertCart(int productId)
        {

            
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
                CartViewModel newCart = new CartViewModel();
                newCart.UserId = userId;
                newCart.CreatedDate = DateTime.Now;
                await cart.Create(newCart);
                return RedirectToAction("InsertCart");
            }
            
            return RedirectToAction("Index", "ShredStore");
        }

    }
}
