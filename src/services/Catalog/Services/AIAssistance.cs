using Catalog.Data;
using Catalog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;

namespace Catalog.Services;

public class AIAssistance(
    IChatClient chatClient,
     CatalogDbContext dbContext,
     IEmbeddingGenerator<string , Embedding<float>> embeddingGenerator,
     IVectorStoreRecordCollection<int , ProductVector> vectorStoreRecordCollection
    )
{
    public async Task<string> SupportAsync(string query)
    {
        var systemPrompt = """
        You are a useful assistant. 
        You always reply with a short and funny message. 
        If you do not know an answer, you say 'I don't know that.' 
        You only answer questions related to outdoor camping products. 
        For any other type of questions, explain to the user that you only answer outdoor camping products questions.
        At the end, Offer one of our products: Hiking Poles-$24, Outdoor Rain Jacket-$12, Outdoor Backpack-$32, Camping Tent-$22
        Do not store memory of the chat conversation.
        """;

        var chatHistory = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, systemPrompt),
            new ChatMessage(ChatRole.User, query)
        };

        var resultPrompt = await chatClient.GetResponseAsync(chatHistory);
        return resultPrompt.Messages[0].Text.ToString()!;
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string query)
    {
        // Check if the collection exists, if not, initialize it
        if (!await vectorStoreRecordCollection.CollectionExistsAsync())
        {
            await InitEmbeddingsAsync();
        }


        // Generate the embedding vector for the query
        var queryEmbedding = await embeddingGenerator.GenerateEmbeddingVectorAsync(query);

        var vectorSearchOptions = new VectorSearchOptions<ProductVector>
        {
            Top = 1,
            VectorPropertyName = "Vector"
        };


        // Perform the vector search
        var results =
            await vectorStoreRecordCollection.VectorizedSearchAsync(queryEmbedding, vectorSearchOptions);

        List<Product> products = [];
        await foreach (var resultItem in results.Results)
        {
            products.Add(new Product
            {
                Id = resultItem.Record.Id,
                Name = resultItem.Record.Name,
                Description = resultItem.Record.Description,
                Price = resultItem.Record.Price,
                ImageUrl = resultItem.Record.ImageUrl
            });
        }

        return products;
    }

    private async Task InitEmbeddingsAsync()
    {
        await vectorStoreRecordCollection.CreateCollectionIfNotExistsAsync();

        var products = await dbContext.Products.ToListAsync();
        foreach (var product in products)
        {
            var productInfo = $"[{product.Name}] is a product that costs [{product.Price}] and is described as [{product.Description}]";

            var productVector = new ProductVector
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Vector = await embeddingGenerator.GenerateEmbeddingVectorAsync(productInfo)
            };

            await vectorStoreRecordCollection.UpsertAsync(productVector);
        }
    }
}
