namespace AzureMessageQueueSample.StreamingAbstractions;

public interface IStreamSubscriber
{
    Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class;
    Task SubscribeAsync<T>(string topic, string subscription, Func<T, Task> handler, CancellationToken cancellationToken) where T : class;
}