using Models;

namespace DataLibrary.Product
{
    public interface IProductRepository
    {
        Task DeletProduct(int id);
        Task<ProductModel?> GetProduct(int id);
        Task<IEnumerable<ProductModel>> GetProducts();
        Task<IEnumerable<ProductModel>> GetProductsByCategory(string Category);
        Task InsertProduct(ProductModel product);
        Task UpdateProduct(ProductModel product);
    }
}