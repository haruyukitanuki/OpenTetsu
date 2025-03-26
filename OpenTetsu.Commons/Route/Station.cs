using Newtonsoft.Json;

namespace OpenTetsu.Commons.Route;

public class Station
{
    [JsonProperty("distance")]
    public float? Distance;

    [JsonProperty("index")]
    public int? Index;

    [JsonProperty("name")]
    public string? Name;

    [JsonProperty("positionName")]
    public string? PositionName;

    [JsonProperty("stopType")]
    public StopType? StopType;
    
    [JsonProperty("doorDirection")]
    public DoorDirection? DoorDirection;

    [JsonProperty("timings")]
    public StationTimings? Timings;
}