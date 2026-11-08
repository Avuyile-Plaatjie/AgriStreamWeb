using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using System.Threading.Tasks;

public class TriggerIrrigationFunction
{
    private readonly ILogger _logger;

    public TriggerIrrigationFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TriggerIrrigationFunction>();
    }

    [Function("TriggerIrrigation")]
    [QueueOutput("irrigation-commands", Connection = "AzureWebJobsStorage")]
    public async Task<string> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "irrigation/trigger")] HttpRequestData req)
    {
        _logger.LogInformation("Processing irrigation trigger request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync("Irrigation command queued successfully.");

        return requestBody;
    }
}