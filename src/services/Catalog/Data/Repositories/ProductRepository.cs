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
            var product = await _dbContext.Products.Where(p=>p.Id == id).FirstOrDefaultAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }


        public async Task<IEnumerable<Product>> GetMatchingProduct(string query)
        {
            return await _dbContext.Products
                .Where(p => p.Name.Contains(query))
                .ToListAsync();      
        }

    }
}
