using ShredStore.Models;

namespace ShredStore.Services
{
    public interface ICartHttpService
    {
        Task<CartViewModel> Create(CartViewModel cart);
        Task Delete(int id);
        Task<CartViewModel> GetById(int id);
    }
}