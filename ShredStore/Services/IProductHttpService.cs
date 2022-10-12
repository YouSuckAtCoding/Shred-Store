using ShredStore.Models;

namespace ShredStore.Services
{
    public interface IProductHttpService
    {
        Task<ProductViewModel> Create(ProductViewModel product);
        Task Delete(int id);
        Task<ProductViewModel> Edit(ProductViewModel product);
        Task<IEnumerable<ProductViewModel>> GetAll();
        Task<ProductViewModel> GetById(int id);
    }
}