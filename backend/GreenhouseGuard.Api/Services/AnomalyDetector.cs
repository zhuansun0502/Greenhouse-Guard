using GreenhouseGuard.Api.Models;

namespace GreenhouseGuard.Api.Services;

public class AnomalyDetector {
    private const int KeepMemoryCount = 20;
    private const double Threshold = 2.5;
    private const int MinimumReadingsCount = 10;

    private readonly Queue<double> lastTwentyValues = new();

    public bool IsAnomaly(double reading, out double zScore) {
        zScore = 0;

        if (lastTwentyValues.Count < MinimumReadingsCount) {
            lastTwentyValues.Enqueue(reading);
            return false;
        }

        double mean = lastTwentyValues.Average();
        double sumSquares = lastTwentyValues.Sum(value => Math.Pow(value - mean, 2));
        double deviation = Math.Sqrt(sumSquares / lastTwentyValues.Count);

        if (deviation != 0)
            zScore = (reading - mean) / deviation;

        if (lastTwentyValues.Count >= KeepMemoryCount)
            lastTwentyValues.Dequeue();
        lastTwentyValues.Enqueue(reading);

        return Math.Abs(zScore) > Threshold;
    }
}
