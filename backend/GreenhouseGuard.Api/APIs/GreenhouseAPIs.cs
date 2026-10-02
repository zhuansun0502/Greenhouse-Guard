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

        app.MapPost("/api/readings", async (SensorReading reading, SensorDataStorage dataStore, AnomalyDetector detector, IHubContext<GreenhouseHub, IGreenhouseClient> hub) =>
        {
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