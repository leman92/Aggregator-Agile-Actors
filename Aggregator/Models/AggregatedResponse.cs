namespace Aggregator.Models;

public class AggregatedResponse
{
    public string Message { get; set; }
    public NewsResponse? News { get; set; }
    public Commodity? Commodity { get; set; }
    public GithubUser? Github { get; set; }
    public List<string>? Errors { get; set; } 
}