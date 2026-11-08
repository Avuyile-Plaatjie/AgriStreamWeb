using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

public class QueueTriggerFunction
{
    private readonly ILogger<QueueTriggerFunction> _logger;

    public QueueTriggerFunction(ILogger<QueueTriggerFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(QueueTriggerFunction))]
    public void Run(
        [QueueTrigger("irrigation-commands", Connection = "AzureWebJobsStorage")] string message)
    {
        _logger.LogInformation($"Queue trigger function processed message: {message}");
       
    }
}