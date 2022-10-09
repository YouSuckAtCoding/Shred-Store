using Models;

namespace DataLibrary.Cart
{
    public interface ICartRepository
    {
        Task DeleteCart(int id);
        Task<CartModel?> GetCart(int id);
        Task InsertCart(CartModel cart);
    }
}