namespace Aggregator.Models.Requests;

public class AggregationRequest
{
    public string? newsSearchTerm { get; set; } = string.Empty;

    public string? commodityType { get; set; } = string.Empty;

    public string? githubUsername { get; set; } = string.Empty;
}