using Basket.ApiClient;
using Basket.EndPoints;
using Basket.Services;
using ServiceDefaults;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.AddRedisDistributedCache(connectionName: "cache");
builder.Services.AddScoped<BasketService>();

builder.Services.AddHttpClient<CatalogApiClient>(client =>
{
    client.BaseAddress = new Uri("http://catalog");
});


builder.Services.AddMassTransit(Assembly.GetExecutingAssembly());


// keyclock registeration
builder.Services.AddAuthentication("Bearer")
                .AddKeycloakJwtBearer(
                serviceName: "keyClock",
                realm: "SimplyShop",
                configureOptions: options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = "account";

                });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapDefaultEndpoints();

app.MapBasketEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();
