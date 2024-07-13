namespace AzureMessageQueueSample.Exceptions;

public class AzureServiceBusConfigNotFound : Exception
{
    private static readonly string message = "Azure bus service config was not found while registering services";
    public AzureServiceBusConfigNotFound() : base(message)
    {
    }
}