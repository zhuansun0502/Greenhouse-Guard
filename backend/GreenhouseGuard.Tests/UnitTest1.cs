using GreenhouseGuard.Api.DataStore;
using GreenhouseGuard.Api.Models;
using GreenhouseGuard.Api.Services;

namespace GreenhouseGuard.Tests;

public class UnitTest1
{
    [Fact]
    public void AnomalyDetector_WithSpike_ReturnsAnomaly()
    {
        var rnd = new Random();

        SensorDataStorage dataStore = new();
        double baseTemp = 24.0, baseHum = 65.0;
        int baseCo2 = 650;

        for (int i = 0; i < 10; i++)
        {
            baseTemp += rnd.NextDouble() * 0.6 - 0.3;
            baseHum += rnd.NextDouble() * 1.0 - 0.5;
            baseCo2 += rnd.Next(-10, 11);

            dataStore.AddReading(new SensorReading
            {
                Timestamp = DateTime.UtcNow.AddSeconds(i),
                Temperature = (decimal)Math.Round(baseTemp, 1),
                Humidity = (decimal)Math.Round(baseHum, 1),
                Co2Ppm = baseCo2
            });
        }

        AnomalyDetector detector = new(dataStore);
        var spike = new SensorReading
        {
            Timestamp = DateTime.UtcNow.AddSeconds(10),
            Temperature = 48.5m,
            Humidity = 18.0m,
            Co2Ppm = 4800
        };

        Queue<Anomaly> anomalies = detector.CheckForAnomalies(spike);
        Assert.Equal(3, anomalies.Count);
        Assert.Contains(anomalies, a => a.SensorType == "temperature" && a.Value == 48.5m);
        Assert.Contains(anomalies, a => a.SensorType == "humidity" && a.Value == 18.0m);
        Assert.Contains(anomalies, a => a.SensorType == "co2" && a.Value == 4800);
        Assert.All(anomalies, a => Assert.True(Math.Abs(a.ZScore) > 2.5m));
    }
}