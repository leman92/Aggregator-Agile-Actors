using Aggregator.Models;

namespace Aggregator.Services.Abstract;

public interface INewsService
{
    Task<NewsResponse?> GetNewsArticlesAsync(string searchTerm);
}