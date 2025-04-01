using Basket.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Services;

public class BasketService(IDistributedCache cache)
{
    public async Task<ShoppingCart?> GetBasketAsync(string userName)
    {
        var key =await cache.GetStringAsync(userName);

        return string.IsNullOrEmpty(key) ? null : JsonSerializer.Deserialize<ShoppingCart>(key);
    }

    public async Task UpdateBasket(ShoppingCart cart)
    {
        await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart));
    }

    public async Task DeleteBasketAsync(string userName)
    {
        await cache.RemoveAsync(userName);
    }
}
