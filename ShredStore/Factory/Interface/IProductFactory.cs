using ShredStore.Models;

namespace ShredStore.Factory.Interface
{
    public interface IProductFactory
    {
        Task<ProductViewModel> createProduct(ProductViewModel productInfo);
    }
}
