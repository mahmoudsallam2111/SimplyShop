using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ServiceDefaults
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransit(this IServiceCollection services,params Assembly[] assemblies)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.SetInMemorySagaRepositoryProvider();

                x.AddConsumers(assemblies);
                x.AddSagaStateMachines(assemblies);
                x.AddSagas(assemblies);
                x.AddActivities(assemblies);
                x.UsingRabbitMq((context, cfg) =>
                {
                  var configuration = context.GetRequiredService<IConfiguration>();
                  var connectionString = configuration.GetConnectionString("rabbitmq");

                    cfg.Host(connectionString);
                    cfg.ConfigureEndpoints(context);

                });
            });
           return services;
        }
    }
}
