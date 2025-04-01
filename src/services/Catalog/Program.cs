using Catalog.Data;
using Catalog.Endpoints;
using Catalog.Services;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddServiceDefaults();


// connect and register the catalog database
builder.AddSqlServerDbContext<CatalogDbContext>(connectionName: "catalogdb");

builder.Services.AddScoped<ProductService>();
builder.Services.AddCatalogServices();


var app = builder.Build();


// Configure the HTTP request pipeline.

app.MapDefaultEndpoints();

app.UseMigration();

app.MapProductEndpoints();

app.UseHttpsRedirection();


app.Run();

