using System.Text.Json;
using Aggregator.Models;
using Aggregator.Services.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace Aggregator.Services;

public class CommodityService : ICommodityService
{
   private readonly IHttpClientFactory _httpClientFactory;
   private readonly IConfiguration _configuration;
   private readonly IMemoryCache _memoryCache;

   public CommodityService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
   {
      _httpClientFactory = httpClientFactory;
      _configuration = configuration;
      _memoryCache = memoryCache;
   }
   
   
   public async Task<Commodity?> GetCommodityAsync(string commodityType)
   {
      if (_memoryCache.TryGetValue(commodityType, out string cachedData))
      {
         return JsonSerializer.Deserialize<Commodity>(cachedData); 
      }
      
      var client = _httpClientFactory.CreateClient();
      
      var apiKey = _configuration["ApiKeys:ApiNinja"]!;
      var baseurl = _configuration["ApiUrls:ApiNinja"]!;
      
      var url = $"{baseurl}/commodityprice?name={commodityType}";
       
      client.DefaultRequestHeaders.Add("x-api-key", apiKey);
      
      var response = await client.GetAsync(url);
      response.EnsureSuccessStatusCode();
    
      var json = await response.Content.ReadAsStringAsync();
      
      _memoryCache.Set(commodityType, json, TimeSpan.FromMinutes(10));
      
      var data =  JsonSerializer.Deserialize<Commodity>(json);
      return data;
   }
}