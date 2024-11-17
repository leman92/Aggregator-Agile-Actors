using Aggregator.Models;

namespace Aggregator.Services.Abstract;

public interface IGithubService
{
    Task<GithubUser?> GetGithubUserAsync(string username);
}