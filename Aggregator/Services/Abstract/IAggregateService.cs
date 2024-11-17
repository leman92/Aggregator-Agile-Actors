using Aggregator.Models;

namespace Aggregator.Services.Abstract;

public interface IAggregateService
{
    Task<(NewsResponse? newsResponse,Commodity? commodity,GithubUser? user,List<string> errors)> GetAggregateDataAsync(string newsSearchTerm,string commodityType,string githubUsername);
}