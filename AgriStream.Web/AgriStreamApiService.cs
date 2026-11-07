using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

public class AgriStreamApiService
{
    private readonly HttpClient _httpClient;

    public AgriStreamApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"]);
    }

    public async Task<string> GetTelemetryAsync(string fieldId)
    {
        return await _httpClient.GetStringAsync($"/api/telemetry/{fieldId}");
    }

    public async Task TriggerIrrigationAsync(object payload)
    {
        await _httpClient.PostAsJsonAsync("/api/irrigation/trigger", payload);
    }
}