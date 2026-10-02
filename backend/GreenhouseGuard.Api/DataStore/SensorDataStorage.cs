using GreenhouseGuard.Api.Models;

namespace GreenhouseGuard.Api.DataStore;

public class SensorDataStorage
{
    private readonly Queue<SensorReading> sensorReadings = [];
    private readonly List<Anomaly> anomalies = [];
    private int sequenceNumber = 0;

    public void AddReading(SensorReading reading)
    {
        reading.Id = Guid.NewGuid();
        reading.SequenceNumber = sequenceNumber++;
        sensorReadings.Enqueue(reading);
    }

    public Queue<SensorReading> GetSensorReadings() => sensorReadings;

    // public void AddAnomaly(Anomaly anomaly)
    // {
    //     anomaly.Id = Guid.NewGuid();
    //     anomalies.Add(anomaly);
    // }

    public void AddAnomalies(List<Anomaly> anomalies)
    {
        this.anomalies.AddRange(anomalies);
    }

    public List<Anomaly> GetAnomalies() => anomalies;
}