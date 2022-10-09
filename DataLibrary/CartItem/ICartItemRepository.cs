using Models;

namespace DataLibrary.CartItem
{
    public interface ICartItemRepository
    {
        Task DeleteCartItem(int id);
        Task<IEnumerable<CartItemModel>> GetCartItems(int id);
        Task InsertCartItem(CartItemModel cartItem);
        Task UpdateCartItem(CartItemModel cartItem);
    }
}