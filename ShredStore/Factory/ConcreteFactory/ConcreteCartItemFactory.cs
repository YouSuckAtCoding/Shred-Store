using ShredStore.Factory.Interface;
using ShredStore.Models;

namespace ShredStore.Factory.ConcreteFactory
{
    public class ConcreteCartItemFactory : ICartItemFactory
    {
        public CartItemViewModel CreateCartItem(CartViewModel item, int productId)
        {
            CartItemViewModel cartItemViewModel = new CartItemViewModel();
            cartItemViewModel.ProductId = productId;
            cartItemViewModel.CartId = item.Id;
            return cartItemViewModel;
        }
    }
}
