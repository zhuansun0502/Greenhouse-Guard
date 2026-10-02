using GreenhouseGuard.Api.Models;

namespace GreenhouseGuard.Api.DataStore;

public class SensorDataStorage
{
    private readonly Queue<SensorReading> sensorReadings = [];
    private readonly Queue<Anomaly> anomalies = [];
    private int sequenceNumber = 0;
    private readonly int keepMemoryCount = 20;
    private readonly Lock _lock = new();

    public void AddReading(SensorReading reading)
    {
        lock (_lock)
        {
            reading.Id = Guid.NewGuid();
            reading.SequenceNumber = sequenceNumber++;
            sensorReadings.Enqueue(reading);
            if (sensorReadings.Count > keepMemoryCount)
                sensorReadings.Dequeue();
        }
    }

    public Queue<SensorReading> GetSensorReadings()
    {
        lock (_lock)
        {
            return sensorReadings;
        }
    }

    public SensorReading? GetLatestReading()
    {
        lock (_lock)
        {
            return sensorReadings.LastOrDefault();
        }
    }

    public void AddAnomaly(Anomaly anomaly)
    {
        lock (_lock)
        {
            anomalies.Enqueue(anomaly);
            if (anomalies.Count > keepMemoryCount)
                anomalies.Dequeue();
        }
    }

    public void AddAnomalies(Queue<Anomaly> anomalies)
    {
        lock (_lock)
        {
            foreach (var anomaly in anomalies)
            {
                AddAnomaly(anomaly);
            }
        }
    }

    public Queue<Anomaly> GetAnomalies()
    {
        lock (_lock)
        {
            return anomalies;
        }
    }
}