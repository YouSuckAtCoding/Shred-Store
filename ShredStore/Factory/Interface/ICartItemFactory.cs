using ShredStore.Models;

namespace ShredStore.Factory.Interface
{
    public interface ICartItemFactory
    {
        CartItemViewModel CreateCartItem(CartViewModel item, int productId);
    }
}
