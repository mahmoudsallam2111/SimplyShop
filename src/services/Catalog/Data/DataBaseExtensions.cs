using Catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data;

public static class DataBaseExtensions
{
    public static void UseMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        context.Database.Migrate();

        DataSeeder.Seed(context);

    }
}

internal static class DataSeeder
{
    public static void Seed(CatalogDbContext context)
    {
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                        new Product { Name = "Wireless Noise-Canceling Headphones", Description = "Experience high-quality sound with active noise cancellation.", Price = 129.99m, ImageUrl = "product2.png" },
                        new Product { Name = "Smart Fitness Tracker", Description = "Track your steps, heart rate, and sleep patterns effortlessly.", Price = 49.99m, ImageUrl = "product3.png" },
                        new Product { Name = "Portable Espresso Machine", Description = "Enjoy fresh espresso anywhere, anytime.", Price = 89.99m, ImageUrl = "product4.png" },
                        new Product { Name = "Waterproof Bluetooth Speaker", Description = "Great sound quality with a durable waterproof design.", Price = 39.99m, ImageUrl = "product5.png" },
                        new Product { Name = "Ergonomic Office Chair", Description = "Adjustable lumbar support for long hours of comfort.", Price = 199.99m, ImageUrl = "product6.png" },
                        new Product { Name = "4K Action Camera", Description = "Capture stunning videos and photos in ultra HD.", Price = 149.99m, ImageUrl = "product7.png" },
                        new Product { Name = "Multi-Tool Pocket Knife", Description = "A must-have for camping and outdoor adventures.", Price = 29.99m, ImageUrl = "product8.png" },
                        new Product { Name = "Cordless Electric Screwdriver", Description = "Make your DIY projects easier with this powerful tool.", Price = 59.99m, ImageUrl = "product9.png" },
                        new Product { Name = "Smart LED Light Bulb", Description = "Control lighting with your voice or smartphone.", Price = 24.99m, ImageUrl = "product10.png" },
                        new Product { Name = "Rechargeable Hand Warmer", Description = "Stay warm in cold weather with this portable device.", Price = 34.99m, ImageUrl = "product11.png" }
            );
            context.SaveChanges();
        }
    }
}