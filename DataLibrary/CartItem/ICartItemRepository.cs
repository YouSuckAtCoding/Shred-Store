using Models;

namespace DataLibrary.CartItem
{
    public interface ICartItemRepository
    {
        Task DeleteCartItem(int productId, int amount, int cartId);
        Task<IEnumerable<CartItemModel>> GetCartItems(int id);
        Task InsertCartItem(CartItemModel cartItem);
        Task UpdateCartItem(CartItemModel cartItem);
    }
}