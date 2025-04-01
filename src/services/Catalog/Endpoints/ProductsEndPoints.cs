using Catalog.Models;
using Catalog.Services;

namespace Catalog.Endpoints;

public static class ProductsEndPoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products");

        // GET all
        group.MapGet("/", async (ProductService service) =>
        {
            var products = await service.GetProductsAsync();
            return Results.Ok(products);
        })
        .WithName("GetAllProducts")
        .Produces<List<Product>>(StatusCodes.Status200OK);

        // GET by ID
        group.MapGet("/{id}", async (int id, ProductService service) =>
        {
            var product = await service.GetProductAsync(id);
            if (product is null) return Results.NotFound();

            return Results.Ok(product);
        })
        .WithName("GetProductById")
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // POST (Create)
        group.MapPost("/", async (Product product, ProductService service) =>
        {
            await service.AddProductAsync(product);
            return Results.Created($"/products/{product.Id}", product);
        })
        .WithName("CreateProduct")
        .Produces<Product>(StatusCodes.Status201Created);

        // PUT (Update)
        group.MapPut("/{id}", async (int id, Product inputProduct, ProductService service) =>
        {
            var updatedProduct = await service.GetProductAsync(id);

            updatedProduct.Name = inputProduct.Name;
            updatedProduct.Description = inputProduct.Description;
            updatedProduct.Price = inputProduct.Price;
            updatedProduct.ImageUrl = inputProduct.ImageUrl;

            if (updatedProduct is null) return Results.NotFound();

            await service.UpdateProductAsync(updatedProduct);
            return Results.NoContent();
        })
        .WithName("UpdateProduct")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        // DELETE
        group.MapDelete("/{id}", async (int id, ProductService service) =>
        {
            await service.DeleteProductAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteProduct")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

    }
}
