using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


#region Backing Services

var sqlServer = builder
                    .AddSqlServer("sqlServer")
                    .WithDataVolume()
                    .WithEnvironment("ACCEPT_EULA", "Y")
                    .WithLifetime(ContainerLifetime.Persistent);



var catalogDb = sqlServer.AddDatabase("catalogdb");


var redisDb = builder.AddRedis("cache")
                     .WithRedisInsight()
                     .WithDataVolume()
                     .WithLifetime(ContainerLifetime.Persistent);


#endregion

#region projects

var catalog =  builder.AddProject<Projects.Catalog>("catalog")
                      .WithReference(catalogDb)
                      .WaitFor(catalogDb);

var basket =  builder.AddProject<Projects.Basket>("basket")
                     .WithReference(redisDb)
                     .WaitFor(redisDb);



#endregion

#region



#endregion


builder.Build().Run();
