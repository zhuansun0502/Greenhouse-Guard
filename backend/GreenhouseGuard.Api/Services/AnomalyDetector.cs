using GreenhouseGuard.Api.DataStore;
using GreenhouseGuard.Api.Models;

namespace GreenhouseGuard.Api.Services;

public class AnomalyDetector(SensorDataStorage dataStore)
{
    private const double Threshold = 2.5;
    private const int MinimumReadingsCount = 10;

    public bool IsAnomaly(double latestReadingData, List<double> historyReadings, out double zScore)
    {
        zScore = 0;
        double mean = historyReadings.Average();
        double deviation = Math.Sqrt(historyReadings.Sum(value => Math.Pow(value - mean, 2)) / historyReadings.Count);
        if (deviation != 0)
            zScore = (latestReadingData - mean) / deviation;
        return Math.Abs(zScore) > Threshold;
    }

    public Queue<Anomaly> CheckForAnomalies(SensorReading latestReading)
    {
        Queue<Anomaly> anomalies = [];
        List<SensorReading> historyReadings = [.. dataStore.GetSensorReadings()];

        if (historyReadings.Count < MinimumReadingsCount)
            return [];

        (string sensorType, decimal latestReadingValueBySensor, Func<SensorReading, decimal> getValue)[] readingsMap =
        [
            ("temperature", latestReading.Temperature, r => r.Temperature),
            ("humidity",    latestReading.Humidity,    r => r.Humidity),
            ("co2",         latestReading.Co2Ppm,      r => r.Co2Ppm)    ];

        foreach (var (sensorType, latestReadingValueBySensor, getValue) in readingsMap)
        {
            List<double> historyReadingsBySensor = [.. historyReadings.Select(r => (double)getValue(r))];
            if (IsAnomaly((double)latestReadingValueBySensor, historyReadingsBySensor, out double zScore))
            {
                anomalies.Enqueue(new Anomaly
                {
                    Id = Guid.NewGuid(),
                    DetectedAt = latestReading.Timestamp,
                    Value = latestReadingValueBySensor,
                    ZScore = (decimal)zScore,
                    SensorType = sensorType,
                    Reason = $"Anomaly detected in {sensorType} sensor with z-score {zScore}"
                });
            }
        }

        return anomalies;
    }
}