using Catalog.Data;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddServiceDefaults();


// connect and register the catalog database
builder.AddSqlServerDbContext<CatalogDbContext>(connectionName: "catalogdb");


var app = builder.Build();


// Configure the HTTP request pipeline.

app.MapDefaultEndpoints();

app.UseMigration();


app.UseHttpsRedirection();


app.Run();

