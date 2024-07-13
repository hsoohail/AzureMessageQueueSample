using AzureMessageQueueSample.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureMessageQueueSample;

public static class Extension
{
    public static IServiceCollection AddAzureServiceBus(this IServiceCollection services,
        IConfiguration configuration)
    {
        var azureServiceBusConfig = configuration.GetSection(nameof(AzureServiceBusConfig)).Get<AzureServiceBusConfig>();

        if (azureServiceBusConfig == null)
            throw new AzureServiceBusConfigNotFound();

        services.AddSingleton(azureServiceBusConfig);
        return services;
    }
}