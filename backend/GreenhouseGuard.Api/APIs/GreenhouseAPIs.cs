using System.Text.Json;
using GreenhouseGuard.Api.DataStore;
using GreenhouseGuard.Api.Hubs;
using GreenhouseGuard.Api.Models;
using GreenhouseGuard.Api.Services;
using Microsoft.AspNetCore.SignalR;

namespace GreenhouseGuard.Api.APIs;

public static class GreenhouseAPIs
{
    public static void MapGreenhouseAPIs(WebApplication app)
    {
        app.MapGet("/api/readings/latest", (SensorDataStorage dataStore) =>
        {
            if (dataStore.GetLatestReading() is null)
                return Results.NoContent();

            return Results.Ok(dataStore.GetLatestReading());
        });

        app.MapPost("/api/readings", async (HttpRequest request, SensorDataStorage dataStore, AnomalyDetector detector, IHubContext<GreenhouseHub, IGreenhouseClient> hub) =>
            {
                JsonElement body;
                try
                {
                    JsonDocument doc = await JsonDocument.ParseAsync(request.Body);
                    body = doc.RootElement.Clone();
                }
                catch (JsonException ex)
                {
                    return Results.BadRequest($"Request body is not valid JSON: {ex.Message}");
                }

                if (!body.TryGetProperty("timestamp", out var ts) || ts.ValueKind != JsonValueKind.String || !ts.TryGetDateTime(out var timestamp))
                    return Results.BadRequest("timestamp must be a valid date string.");
                if (!body.TryGetProperty("temperature", out var temperature) || temperature.ValueKind != JsonValueKind.Number)
                    return Results.BadRequest("temperature must be a number.");
                if (!body.TryGetProperty("humidity", out var hum) || !hum.TryGetDecimal(out var humidity) || humidity < 0)
                    return Results.BadRequest("humidity must be a non-negative number.");
                if (!body.TryGetProperty("co2", out var co2) || !co2.TryGetInt32(out var co2Ppm) || co2Ppm < 0)
                    return Results.BadRequest("co2 must be a non-negative integer.");

                var reading = new SensorReading
                {
                    Timestamp = timestamp,
                    Temperature = temperature.GetDecimal(),
                    Humidity = humidity,
                    Co2Ppm = co2Ppm
                };
                Queue<Anomaly> anomalies = detector.CheckForAnomalies(reading);
                dataStore.AddAnomalies(anomalies);
                dataStore.AddReading(reading);

                await hub.Clients.All.NewReading(reading);
                if (anomalies.Count > 0)
                    await hub.Clients.All.AnomaliesDetected(anomalies);

                return Results.Ok();
            });

        app.MapGet("/api/anomalies", (SensorDataStorage dataStore) =>
        {
            return Results.Ok(dataStore.GetAnomalies());
        });
    }
}