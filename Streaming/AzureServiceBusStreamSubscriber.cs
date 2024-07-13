using Azure.Messaging.ServiceBus;
using AzureMessageQueueSample.StreamingAbstractions;
using Microsoft.Extensions.Logging;

namespace AzureMessageQueueSample.Streaming;

public class AzureServiceBusStreamSubscriber : IStreamSubscriber
{
    private readonly ILogger<AzureServiceBusStreamSubscriber> _logger;
    private readonly AzureServiceBusConfig _config;

    public AzureServiceBusStreamSubscriber(ILogger<AzureServiceBusStreamSubscriber> logger,
        AzureServiceBusConfig config)
    {
        _logger = logger;
        _config = config;
    }

    public async Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class
    {
        _logger.LogInformation("This version of subscriber is not supported by Azure Service Bus");
    }

    public async Task SubscribeAsync<T>(string topic, string subscription, Func<T, Task> handler, CancellationToken cancellationToken) where T : class
    {
        var serviceBusClient = new ServiceBusClient(_config.ConnectionString);
        var serviceBusProcessorOptions = new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 1,
            AutoCompleteMessages = false,
        };
        var serviceBusProcessor = serviceBusClient.CreateProcessor(topic, subscription, serviceBusProcessorOptions);
        serviceBusProcessor.ProcessMessageAsync += async (args) => await ProcessMessageAsync(args, handler, cancellationToken);
        serviceBusProcessor.ProcessErrorAsync += ProcessErrorAsync;
        await serviceBusProcessor.StartProcessingAsync(cancellationToken);
    }

    private async Task ProcessMessageAsync<T>(ProcessMessageEventArgs args, Func<T, Task> handler, CancellationToken cancellationToken = default)
    {
        var payload = args.Message.Body.ToObjectFromJson<T>();
        await handler(payload);
        await args.CompleteMessageAsync(args.Message, cancellationToken);
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        _logger.LogError(arg.Exception, $"Message processing failed");
        _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
        _logger.LogDebug($"- Entity Path: {arg.EntityPath}");

        return Task.CompletedTask;
    }
}