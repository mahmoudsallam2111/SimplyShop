using Catalog.Data;
using Catalog.Endpoints;
using Catalog.Models;
using Catalog.Services;
using Microsoft.SemanticKernel;
using ServiceDefaults;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddServiceDefaults();


// connect and register the catalog database
builder.AddSqlServerDbContext<CatalogDbContext>(connectionName: "catalogdb");

//ollama  
builder.AddOllamaSharpChatClient("ollama-llama3-2");


// embedding 
builder.AddOllamaSharpEmbeddingGenerator("ollama-all-minilm");

// vector store
builder.Services.AddInMemoryVectorStoreRecordCollection<int, ProductVector>("products");

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<AIAssistance>();
builder.Services.AddCatalogServices();



builder.Services.AddMassTransit(Assembly.GetExecutingAssembly());


var app = builder.Build();


// Configure the HTTP request pipeline.

app.MapDefaultEndpoints();

app.UseMigration();

app.MapProductEndpoints();

app.UseHttpsRedirection();


app.Run();

