using Aggregator.Models;

namespace Aggregator.Services.Abstract;

public interface IStatisticsService
{
    void LogRequest(string apiName, TimeSpan responseTime);
    
    ApiStatistics GetStatistics(string apiName);
}