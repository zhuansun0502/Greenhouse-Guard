using GreenhouseGuard.Api.DataStore;
using GreenhouseGuard.Api.Models;
using GreenhouseGuard.Api.Services;

namespace GreenhouseGuard.Api.APIs;

public static class GreenhouseAPIs
{
    public static void MapGreenhouseAPIs(WebApplication app)
    {
        app.MapGet("/api/readings/latest", (SensorDataStorage dataStore) =>
        {
            return Results.Ok(dataStore.GetSensorReadings());
        });

        app.MapPost("/api/readings", (SensorReading reading, SensorDataStorage dataStore, AnomalyDetector detector) =>
        {
            var anomalies = detector.CheckForAnomalies(reading);
            dataStore.AddAnomalies([.. anomalies]);
            dataStore.AddReading(reading);
            return Results.Ok();
        });

        app.MapGet("/api/anomalies", (SensorDataStorage dataStore) =>
        {
            // return Results.Ok(dataStore.GetSensorReading(id));
        });
    }
}