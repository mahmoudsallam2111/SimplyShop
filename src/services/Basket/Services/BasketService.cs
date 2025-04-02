using Basket.ApiClient;
using Basket.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Services;

public class BasketService(IDistributedCache cache , CatalogApiClient catalogApiClient)
{
    public async Task<ShoppingCart?> GetBasketAsync(string userName)
    {
        var key =await cache.GetStringAsync(userName);

        return string.IsNullOrEmpty(key) ? null : JsonSerializer.Deserialize<ShoppingCart>(key);
    }

    public async Task UpdateBasket(ShoppingCart cart)
    {

        foreach (var item in cart.ShoppingCartItems)
        {
            var product = await catalogApiClient.GetProduct(item.ProductId);
            item.Price = product.Price;
            item.ProductName = product.Name;
        }
        await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart));
    }

    public async Task DeleteBasketAsync(string userName)
    {
        await cache.RemoveAsync(userName);
    }

    public async Task UpdateBasketPrices(int productId, decimal newPrice)
    {
        var basket = await GetBasketAsync("mahmoud");
            var basketItems = basket.ShoppingCartItems.Where(x => x.ProductId == productId).ToList();

        if (basketItems.Any())
        {
            foreach (var item in basketItems)
            {
                    item.Price = newPrice;
            }
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));   
        }
        
    }
}
