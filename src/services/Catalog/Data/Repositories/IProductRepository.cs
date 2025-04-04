using Catalog.Models;

namespace Catalog.Data.repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> GetProductAsync(int id);
    Task CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task<IEnumerable<Product>> GetMatchingProduct(string query);
}
