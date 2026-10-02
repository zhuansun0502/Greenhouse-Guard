using GreenhouseGuard.Api.Models;

namespace GreenhouseGuard.Api.DataStore;

public class SensorDataStorage
{
    private readonly Queue<SensorReading> sensorReadings = [];
    private readonly Queue<Anomaly> anomalies = [];
    private int sequenceNumber = 0;
    private readonly int keepMemoryCount = 20;

    public void AddReading(SensorReading reading)
    {
        reading.Id = Guid.NewGuid();
        reading.SequenceNumber = sequenceNumber++;
        sensorReadings.Enqueue(reading);
        if (sensorReadings.Count > keepMemoryCount)
            sensorReadings.Dequeue();
    }

    public Queue<SensorReading> GetSensorReadings() => sensorReadings;

    public SensorReading? GetLatestReading() => sensorReadings.LastOrDefault();

    public void AddAnomaly(Anomaly anomaly)
    {
        anomalies.Enqueue(anomaly);
        if (anomalies.Count > keepMemoryCount)
            anomalies.Dequeue();
    }

    public void AddAnomalies(Queue<Anomaly> anomalies)
    {
        foreach (var anomaly in anomalies)
        {
            AddAnomaly(anomaly);
        }
    }

    public Queue<Anomaly> GetAnomalies() => anomalies;
}