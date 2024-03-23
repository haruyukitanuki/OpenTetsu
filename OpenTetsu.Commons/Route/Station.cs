using Newtonsoft.Json;

namespace OpenTetsu.Commons.Route;

public class Station
{
    [JsonProperty("distanceFromKmZero")]
    public float? DistanceFromKmZero;

    [JsonProperty("index")]
    public int? Index;

    [JsonProperty("name")]
    public string? Name;

    [JsonProperty("positionName")]
    public string? PositionName;

    [JsonProperty("stopType")]
    public StopType? StopType;

    [JsonProperty("timings")]
    public StationTimings? Timings;
}