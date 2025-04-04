using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


#region Backing Services

var sqlServer = builder
                    .AddSqlServer("sqlServer")
                    //.WithDataVolume()
                    .WithEnvironment("ACCEPT_EULA", "Y")
                    .WithLifetime(ContainerLifetime.Persistent);


var catalogDb = sqlServer.AddDatabase("catalogdb");


var redisDb = builder.AddRedis("cache")
                     .WithRedisInsight()
                    // .WithDataVolume()
                     .WithLifetime(ContainerLifetime.Persistent);



var rabbitmq = builder.AddRabbitMQ("rabbitmq")
                     .WithManagementPlugin()
                    // .WithDataVolume()
                     .WithLifetime(ContainerLifetime.Persistent);



var keyClock = builder.AddKeycloak("keyClock" , 8080)
                     //.WithDataVolume()
                     .WithLifetime(ContainerLifetime.Persistent);



var ollama = builder
                .AddOllama("ollama", 11434)
                .WithDataVolume()
                .WithLifetime(ContainerLifetime.Persistent)
                .WithOpenWebUI();

var llama = ollama.AddModel("llama3.2");
var embedding = ollama.AddModel("all-minilm");


// i.e local development mode , cause Azure Container Apps do not support persistent storage volumes
if (builder.ExecutionContext.IsRunMode)
{
    sqlServer.WithDataVolume();
    redisDb.WithDataVolume();
    rabbitmq.WithDataVolume();
    keyClock.WithDataVolume();
}

#endregion

#region projects

var catalog =  builder.AddProject<Projects.Catalog>("catalog")
                      .WithReference(catalogDb)
                      .WithReference(rabbitmq)
                      .WithReference(llama)
                      .WithReference(embedding)
                      .WaitFor(catalogDb)
                      .WaitFor(rabbitmq)
                      .WaitFor(llama)
                      .WaitFor(embedding);

var basket =  builder.AddProject<Projects.Basket>("basket")
                     .WithReference(redisDb)
                     .WithReference(catalog)     // this is for service discovery
                     .WithReference(rabbitmq)
                     .WithReference(keyClock)
                     .WaitFor(redisDb)
                     .WaitFor(rabbitmq)
                     .WaitFor(keyClock);


var webApp = builder.AddProject<Projects.WebApp>("webapp")
                    .WithExternalHttpEndpoints()
                    .WithReference(catalog)
                    .WithReference(basket)
                    .WithReference(redisDb)
                    .WaitFor(catalog)
                    .WaitFor(basket)
                    .WaitFor(redisDb);

#endregion






#region



#endregion


builder.Build().Run();
