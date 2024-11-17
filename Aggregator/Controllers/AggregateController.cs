using Aggregator.Models;
using Aggregator.Models.Requests;
using Aggregator.Services;
using Aggregator.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Aggregator.Controllers;


[ApiController]
[Route("[controller]")]
public class AggregateController : ControllerBase
{
    private readonly IAggregateService _aggregateService;
    private readonly IStatisticsService _statisticsService;

    public AggregateController(IAggregateService aggregateService, IStatisticsService statisticsService)
    {
        _aggregateService = aggregateService;
        _statisticsService = statisticsService;
    }
    
    
    [HttpGet("aggregation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAggregate([FromQuery] AggregationRequest request)
    {
        var (news,commodity,githubuser,errors) = await _aggregateService.GetAggregateDataAsync(request.newsSearchTerm!,request.commodityType!,request.githubUsername!);
        
        var response = new AggregatedResponse
        {
            Message = errors.Any() ? "Partial success" : "Success",
            News = news,
            Commodity = commodity,
            Github = githubuser,
            Errors = errors
        };

        return Ok(response);
    }
    
    [HttpGet("api-stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetApiStatistics()
    {
        var stats = new Dictionary<string, ApiStatistics>
        {
            { "NewsService", _statisticsService.GetStatistics("NewsService") },
            { "CommodityService", _statisticsService.GetStatistics("CommodityService") },
            { "GithubService", _statisticsService.GetStatistics("GithubService") }
        };

        return Ok(stats);
    }
}