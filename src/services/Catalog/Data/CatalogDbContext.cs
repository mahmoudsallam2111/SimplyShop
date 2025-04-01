using Catalog.Data.config;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data;

public class CatalogDbContext : DbContext
{
    public DbSet<Catalog.Models.Product> Products => Set<Catalog.Models.Product>();
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options):base(options)
    {

    }

    override protected void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(Schemas.catalog);

        builder.ApplyConfiguration(new ProductEntityTypeConfiguration());
    }
}
