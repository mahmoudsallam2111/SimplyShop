using Catalog.Data.repositories;
using Catalog.Data.Repositories;

namespace Catalog.Data
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddCatalogServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
