using Catalog.Data.repositories;
using Catalog.Models;
using MassTransit;
using ServiceDefaults.Messaging.Events;

namespace Catalog.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IBus _bus;

        public ProductService(IProductRepository productRepository , IBus bus)
        {
            _productRepository = productRepository;
            _bus = bus;
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productRepository.GetProductsAsync();
        }
        public async Task<Product> GetProductAsync(int id)
        {
            return await _productRepository.GetProductAsync(id);
        }
        public async Task AddProductAsync(Product product)
        {
            await _productRepository.CreateProductAsync(product);
        }
        public async Task UpdateProductAsync(Product productToUpdate, Product inputProduct)
        {
            if (productToUpdate.Price != inputProduct.Price)
            {
                    var integrationEvent = new ProductPriceChangedIntegrationEvent
                    {
                        ProductId = productToUpdate.Id,
                        Name = inputProduct.Name,
                        Price = inputProduct.Price,
                        Description = inputProduct.Description,
                        ImageUrl = inputProduct.ImageUrl,
                    };

                   await _bus.Publish(integrationEvent);
            }

            productToUpdate.Name = inputProduct.Name;
            productToUpdate.Description = inputProduct.Description;
            productToUpdate.Price = inputProduct.Price;
            productToUpdate.ImageUrl = inputProduct.ImageUrl;

            // raise integraion Event if price has changed

            await _productRepository.UpdateProductAsync(productToUpdate);

        }
        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }

        public async Task<IEnumerable<Product>> SearchProductAsync(string query)
        {
            return await _productRepository.GetMatchingProduct(query);
        }
    }
}
