using GreenhouseGuard.Api.Models;
using Microsoft.AspNetCore.SignalR;

namespace GreenhouseGuard.Api.Hubs;

public interface IGreenhouseClient
{
    Task NewReading(SensorReading reading);
    Task AnomaliesDetected(Queue<Anomaly> anomalies);
}

public class GreenhouseHub : Hub<IGreenhouseClient>
{
}