namespace Aggregator.Models;

using System;

public class Commodity
{
    public string exchange { get; set; }
    public string name { get; set; }
    public decimal price { get; set; }
    public long updated { get; set; }

    // Optional: Helper to convert the "Updated" field (timestamp) to a DateTime
   // public DateTime UpdatedDateTime => DateTimeOffset.FromUnixTimeSeconds(Updated).UtcDateTime;
}
