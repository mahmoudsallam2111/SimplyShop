using Catalog.Models;

namespace Basket.ApiClient
{
    public class CatalogApiClient
    {
        private readonly HttpClient _httpClient;

        public CatalogApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Product> GetProduct(int id)
        {
            var response =  await _httpClient.GetFromJsonAsync<Product>($"/products/{id}");
            return response!;
        }
    }
}
