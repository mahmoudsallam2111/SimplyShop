using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


#region Backing Services

var sqlServer = builder
                    .AddSqlServer("sqlServer")
                    .WithDataVolume()
                    .WithEnvironment("ACCEPT_EULA", "Y")
                    .WithLifetime(ContainerLifetime.Persistent);



var catalogDb = sqlServer.AddDatabase("catalogdb");


#endregion

#region projects

var catalog =  builder.AddProject<Projects.Catalog>("catalog")
                      .WithReference(catalogDb)
                      .WaitFor(catalogDb);


#endregion


builder.Build().Run();
