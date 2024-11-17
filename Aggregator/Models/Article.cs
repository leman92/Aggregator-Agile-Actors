namespace Aggregator.Models;

using System;
using System.Collections.Generic;

public class Article
{
    public long id { get; set; }
    public string title { get; set; }
    public string text { get; set; }
    public string summary { get; set; }
    public string url { get; set; }
    public string image { get; set; }
    public string video { get; set; }
    public DateTime publishDate { get; set; }
    public List<string> authors { get; set; }
    public string language { get; set; }
    public string sourceCountry { get; set; }
    public double sentiment { get; set; }
}

public class NewsResponse
{
    public int offset { get; set; }
    public int number { get; set; }
    public int available { get; set; }
    public List<Article> news { get; set; }
}



