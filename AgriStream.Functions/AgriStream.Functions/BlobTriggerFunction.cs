using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using System;

public class BlobTriggerFunction
{
    private readonly ILogger<BlobTriggerFunction> _logger;

    public BlobTriggerFunction(ILogger<BlobTriggerFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(BlobTriggerFunction))]
    public async Task Run(
        [BlobTrigger("drone-images/{name}", Connection = "AzureWebJobsStorage")] Stream stream,
        string name)
    {
        _logger.LogInformation($"C# Blob trigger processed blob\n Name:{name} \n Size: {stream.Length} bytes");

        string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

       
        var tableClient = new TableClient(connectionString, "AuditLogs");
        await tableClient.CreateIfNotExistsAsync();

       
        var auditEntity = new TableEntity("DroneUpload", Guid.NewGuid().ToString())
        {
            { "FileName", name },
            { "UploadTime", DateTime.UtcNow },
            { "Status", "Processed & Logged" }
        };

        await tableClient.AddEntityAsync(auditEntity);
        _logger.LogInformation($"Audit log successfully written for file: {name}");
    }
}