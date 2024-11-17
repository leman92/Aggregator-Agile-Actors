using System.Collections.Concurrent;
using Aggregator.Models;
using Aggregator.Services.Abstract;

namespace Aggregator.Services;

public class StatisticsService : IStatisticsService
{
    private readonly ConcurrentDictionary<string, ApiStatistics> _stats = new();

    public void LogRequest(string apiName, TimeSpan responseTime)
    {
        var stats = _stats.GetOrAdd(apiName, new ApiStatistics());
        stats.TotalRequests++;
        
        stats.AverageResponseTime = (stats.AverageResponseTime * (stats.TotalRequests - 1) + responseTime.TotalMilliseconds) / stats.TotalRequests;
        
        if (responseTime.TotalMilliseconds < 100)
            stats.PerformanceBuckets["Fast"]++;
        else if (responseTime.TotalMilliseconds < 200)
            stats.PerformanceBuckets["Average"]++;
        else
            stats.PerformanceBuckets["Slow"]++;
    }

    public ApiStatistics GetStatistics(string apiName)
    {
        return _stats.GetValueOrDefault(apiName, new ApiStatistics());
    }
}