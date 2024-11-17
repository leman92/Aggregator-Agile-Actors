using Aggregator.Models;

namespace Aggregator.Services.Abstract;

public interface ICommodityService
{
    Task<Commodity?> GetCommodityAsync(string commodityType);
}