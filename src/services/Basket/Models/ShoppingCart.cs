namespace Basket.Models;

public class ShoppingCart
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItems> ShoppingCartItems { get; set; } = new();
    public decimal TotalPrice => ShoppingCartItems.Sum(i => i.Quantity * i.Price);

}
