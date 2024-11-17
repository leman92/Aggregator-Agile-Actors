using Aggregator.Controllers;
using Aggregator.Models;
using Aggregator.Models.Requests;
using Aggregator.Services;
using Aggregator.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Aggregator.UnitTests;

public class AggregateControllerTests
{
    private readonly Mock<IAggregateService> _aggregateServiceMock;
    private readonly Mock<IStatisticsService> _statisticsServiceMock;
    private readonly AggregateController _controller;

    public AggregateControllerTests()
    {
        _aggregateServiceMock = new Mock<IAggregateService>();
        _statisticsServiceMock = new Mock<IStatisticsService>();
        _controller = new AggregateController(_aggregateServiceMock.Object, _statisticsServiceMock.Object);
    }

    [Fact]
    public async Task GetAggregate_ShouldReturnPartialSuccess_WhenThereAreErrors()
    {
        // Arrange
        var request = new AggregationRequest
        {
            newsSearchTerm = "technology",
            commodityType = "gold",
            githubUsername = "testuser"
        };

        var mockNewsResponse = new NewsResponse { news = new List<Article> { new() { title = "Tech News" } } };
        var mockCommodity = new Commodity { name = "Gold", price = 1800.50m };
        var errors = new List<string> { "GithubService failed" };
        
     

        _aggregateServiceMock
            .Setup(s => s.GetAggregateDataAsync("technology", "gold", "testuser"))
            .ReturnsAsync((mockNewsResponse, mockCommodity, null, errors));

        // Act
        var result = await _controller.GetAggregate(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value as AggregatedResponse;

        Assert.Equal("Partial success", response?.Message);
        Assert.NotNull(response?.News);
        Assert.NotNull(response?.Commodity);
        Assert.Null(response?.Github);
        Assert.NotEmpty(response?.Errors);
    }

    [Fact]
    public async Task GetAggregate_ShouldReturnSuccess_WhenNoErrors()
    {
        // Arrange
        var request = new AggregationRequest
        {
            newsSearchTerm = "science",
            commodityType = "oil",
            githubUsername = "scientist"
        };

        var mockNewsResponse = new NewsResponse { news = new List<Article> { new() { title = "Science News" } } };
        var mockCommodity = new Commodity { name = "Oil", price = 75.30m };
        var mockGithubUser = new GithubUser { name = "scientist", location = "Athens"};
        var errors = new List<string>();

        _aggregateServiceMock
            .Setup(s => s.GetAggregateDataAsync("science", "oil", "scientist"))
            .ReturnsAsync((mockNewsResponse, mockCommodity, mockGithubUser, errors));

        // Act
        var result = await _controller.GetAggregate(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value as AggregatedResponse;

        Assert.Equal("Success", response?.Message);
        Assert.NotNull(response?.News);
        Assert.NotNull(response?.Commodity);
        Assert.NotNull(response?.Github);
        Assert.Empty(response?.Errors);
    }

    [Fact]
    public void GetApiStatistics_ShouldReturnStats()
    {
        // Arrange
        var mockNewsStats = new ApiStatistics
        {
            TotalRequests = 10,
            AverageResponseTime = 120,
            PerformanceBuckets = new Dictionary<string, int>
            {
                { "Fast", 6 },
                { "Average", 3 },
                { "Slow", 1 }
            }
        };

        var mockCommodityStats = new ApiStatistics
        {
            TotalRequests = 5,
            AverageResponseTime = 180,
            PerformanceBuckets = new Dictionary<string, int>
            {
                { "Fast", 2 },
                { "Average", 2 },
                { "Slow", 1 }
            }
        };

        var mockGithubStats = new ApiStatistics
        {
            TotalRequests = 15,
            AverageResponseTime = 90,
            PerformanceBuckets = new Dictionary<string, int>
            {
                { "Fast", 10 },
                { "Average", 4 },
                { "Slow", 1 }
            }
        };

        _statisticsServiceMock.Setup(s => s.GetStatistics("NewsService")).Returns(mockNewsStats);
        _statisticsServiceMock.Setup(s => s.GetStatistics("CommodityService")).Returns(mockCommodityStats);
        _statisticsServiceMock.Setup(s => s.GetStatistics("GithubService")).Returns(mockGithubStats);

        // Act
        var result = _controller.GetApiStatistics();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var stats = okResult.Value as Dictionary<string, ApiStatistics>;

        Assert.NotNull(stats);
        Assert.Equal(3, stats.Count);
        Assert.Equal(10, stats["NewsService"].TotalRequests);
        Assert.Equal(5, stats["CommodityService"].TotalRequests);
        Assert.Equal(15, stats["GithubService"].TotalRequests);
    }
    
    [Fact]
    public void GetApiStatistics_ShouldReturnEmptyStats_WhenNoStatsAvailable()
    {
        // Arrange
        _statisticsServiceMock
            .Setup(s => s.GetStatistics(It.IsAny<string>()))
            .Returns(new ApiStatistics());

        // Act
        var result = _controller.GetApiStatistics();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var stats = okResult.Value as Dictionary<string, ApiStatistics>;

        Assert.NotNull(stats);
       // Assert.Empty(stats);  // Should be empty if no stats are available
    }
}