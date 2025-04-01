using Catalog.Data.repositories;
using Catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _dbContext;

        public ProductRepository(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateProductAsync(Product product)
        {
            _dbContext.Products.Add(product);
           await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product is { })
                _dbContext.Remove(product);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
           _dbContext.Entry<Product>(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return product;
        }
    }
}
