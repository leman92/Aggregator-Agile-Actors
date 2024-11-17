namespace Aggregator.Models;

public class ApiStatistics
{
    public int TotalRequests { get; set; }
    public double AverageResponseTime { get; set; } 
    public Dictionary<string, int> PerformanceBuckets { get; set; } = new Dictionary<string, int>
    {
        { "Fast", 0 },
        { "Average", 0 },
        { "Slow", 0 }
    };
}