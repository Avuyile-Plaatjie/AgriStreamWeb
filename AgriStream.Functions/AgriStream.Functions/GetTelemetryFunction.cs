using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

public class GetTelemetryFunction
{
    private readonly ILogger _logger;

    public GetTelemetryFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<GetTelemetryFunction>();
    }

    [Function("GetTelemetry")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "telemetry/{fieldId}")] HttpRequestData req,
        string fieldId)
    {
        _logger.LogInformation($"Fetching telemetry for field: {fieldId}");

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new { FieldId = fieldId, SoilMoisture = 42.5, Temperature = 24.1, Time = System.DateTime.UtcNow });

        return response;
    }
}