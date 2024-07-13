using AzureMessageQueueSample.StreamingAbstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AzureMessageQueueSample.Streaming;

public static class Extension
{
    public static IServiceCollection AddAzureServiceBusStreaming(this IServiceCollection services)
        => services.AddSingleton<IStreamPublisher, AzureServiceBusStreamPublisher>()
            .AddSingleton<IStreamSubscriber, AzureServiceBusStreamSubscriber>();
}