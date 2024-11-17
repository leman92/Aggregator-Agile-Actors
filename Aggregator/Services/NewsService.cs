using System.Text.Json;
using Aggregator.Models;
using Aggregator.Services.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Polly;

namespace Aggregator.Services;

public class NewsService : INewsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;

    public NewsService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _memoryCache = memoryCache;
    }

    public async Task<NewsResponse?> GetNewsArticlesAsync(string searchTerm)
    {
        if (_memoryCache.TryGetValue(searchTerm, out string cachedData))
        {
            return JsonSerializer.Deserialize<NewsResponse>(cachedData); 
        }
        
        var client = _httpClientFactory.CreateClient();

        var apiKey = _configuration["ApiKeys:NewsApi"]!;
        var baseurl = _configuration["ApiUrls:NewsApi"]!;
        var url = $"{baseurl}/search-news?text={searchTerm}&number=5";
        
        client.DefaultRequestHeaders.Add("x-api-key", apiKey);
       
       var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    
        var json = await response.Content.ReadAsStringAsync();
        
        _memoryCache.Set(searchTerm, json, TimeSpan.FromMinutes(10));
        
        var data =  JsonSerializer.Deserialize<NewsResponse>(json);
        return data;
    }
}