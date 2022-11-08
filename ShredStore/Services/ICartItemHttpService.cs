using ShredStore.Models;

namespace ShredStore.Services
{
    public interface ICartItemHttpService
    {
        Task<CartItemViewModel> Create(CartItemViewModel cartItem);
        Task Delete(int productId, int amount, int cartId);
        Task DeleteAll(int cartId);
        Task<CartItemViewModel> Edit(CartItemViewModel cartItem);
        Task<IEnumerable<CartItemViewModel>> GetAll(int cartId);
    }
}