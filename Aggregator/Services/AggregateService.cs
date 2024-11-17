using Aggregator.Models;
using Aggregator.Services.Abstract;
using Polly;

namespace Aggregator.Services;

public class AggregateService : IAggregateService
{
    private readonly ILogger<AggregateService> _logger;
    private readonly INewsService _newsService;
    private readonly ICommodityService _commodityService;
    private readonly IGithubService _githubService;
    private readonly IStatisticsService _statisticsService;

    public AggregateService(ILogger<AggregateService> logger, INewsService newsService, ICommodityService commodityService, IGithubService githubService, IStatisticsService statisticsService)
    {
        _logger = logger;
        _newsService = newsService;
        _commodityService = commodityService;
        _githubService = githubService;
        _statisticsService = statisticsService;
    }
    
    public async Task<(NewsResponse? newsResponse,Commodity? commodity,GithubUser? user,List<string> errors)>  GetAggregateDataAsync(string newsSearchTerm,string commodityType,string githubUsername)
    {
        var errors = new List<string>();
        NewsResponse? newsData = null;
        Commodity? commodityData = null;
        GithubUser? githubData = null;
        
        var retryPolicy = Policy.Handle<HttpRequestException>()
            .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        
        var taskNews = Task.Run(async () =>
        {
            try
            {
                var startTime = DateTime.Now;
                var result =  await retryPolicy.ExecuteAsync(() => _newsService.GetNewsArticlesAsync(newsSearchTerm));
                var responseTime = DateTime.Now - startTime;
                _statisticsService.LogRequest("NewsService", responseTime);
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogInformation($"NewsService failed: {ex.Message}");
                errors.Add($"NewsService failed: {ex.Message}");
                return null; 
            }
        });
        
        var taskCommodity = Task.Run(async () =>
        {
            try
            {
                var startTime = DateTime.Now;
                var result = await retryPolicy.ExecuteAsync(() => _commodityService.GetCommodityAsync(commodityType));
                var responseTime = DateTime.Now - startTime;
                _statisticsService.LogRequest("CommodityService", responseTime);
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogInformation($"CommodityService failed: {ex.Message}");
                errors.Add($"CommodityService failed: {ex.Message}");
                return null; 
            }
        });

        var taskGithub = Task.Run(async () =>
        {
            try
            {
                var startTime = DateTime.Now;
                var result =  await retryPolicy.ExecuteAsync(() => _githubService.GetGithubUserAsync(githubUsername));
                var responseTime = DateTime.Now - startTime;
                _statisticsService.LogRequest("GithubService", responseTime);
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogInformation($"GithubService failed: {ex.Message}");
                errors.Add($"GithubService failed: {ex.Message}");
                return null; 
            }
        });

        
        await Task.WhenAll(taskNews, taskCommodity, taskGithub);
        
        newsData = await taskNews;
        commodityData = await taskCommodity;
        githubData = await taskGithub;

        return (newsData, commodityData,githubData, errors);
      
    }

}