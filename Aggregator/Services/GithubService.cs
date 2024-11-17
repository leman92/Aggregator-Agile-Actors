using System.Text.Json;
using Aggregator.Models;
using Aggregator.Services.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace Aggregator.Services;

public class GithubService : IGithubService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;

    public GithubService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _memoryCache = memoryCache;
    }
    
    public async Task<GithubUser?> GetGithubUserAsync(string username)
    {
        if (_memoryCache.TryGetValue(username, out string cachedData))
        {
            return JsonSerializer.Deserialize<GithubUser>(cachedData); 
        }
        
        var client = _httpClientFactory.CreateClient();
        
        var baseurl = _configuration["ApiUrls:GitHubApi"]!;
        var url = $"{baseurl}/users/{username}";
        
        client.DefaultRequestHeaders.Add("User-Agent", "request");
        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    
        var json = await response.Content.ReadAsStringAsync();
        
        _memoryCache.Set(username, json, TimeSpan.FromMinutes(10));
        
        var data =  JsonSerializer.Deserialize<GithubUser>(json);
        return data;
    }
}