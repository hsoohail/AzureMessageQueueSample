using System.Text.Json;
using Azure.Messaging.ServiceBus;
using AzureMessageQueueSample.StreamingAbstractions;

namespace AzureMessageQueueSample.Streaming;

public class AzureServiceBusStreamPublisher : IStreamPublisher
{
    private readonly AzureServiceBusConfig _config;

    public AzureServiceBusStreamPublisher(AzureServiceBusConfig config)
    {
        _config = config;
    }

    public async Task PublishAsync<T>(string topic, T data) where T : class
    {
        await using var serviceBusClient = new ServiceBusClient(_config.ConnectionString);
        var serviceBusSender = serviceBusClient.CreateSender(topic);

        var message = JsonSerializer.Serialize(data);
        var serviceBusMessage = new ServiceBusMessage(message);

        await serviceBusSender.SendMessageAsync(serviceBusMessage);
    }
}