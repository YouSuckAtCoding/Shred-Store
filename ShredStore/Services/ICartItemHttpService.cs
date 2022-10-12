using ShredStore.Models;

namespace ShredStore.Services
{
    public interface ICartItemHttpService
    {
        Task<CartItemViewModel> Create(CartItemViewModel cartItem);
        Task Delete(int id);
        Task<CartItemViewModel> Edit(CartItemViewModel cartItem);
        Task<IEnumerable<CartItemViewModel>> GetAll();
        Task<CartItemViewModel> GetById(int id);
    }
}