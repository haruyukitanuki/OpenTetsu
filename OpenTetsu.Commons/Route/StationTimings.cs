using Newtonsoft.Json;

namespace OpenTetsu.Commons.Route;

public class StationTimings
{
    [JsonProperty("arrival")]
    public DateTime? Arrival;

    [JsonProperty("departure")]
    public DateTime? Departure;
}