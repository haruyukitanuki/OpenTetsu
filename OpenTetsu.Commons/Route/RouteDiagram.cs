using Newtonsoft.Json;

namespace OpenTetsu.Commons.Route;

public class Diagram
{
    [JsonProperty("direction")]
    public Direction? Direction;

    [JsonProperty("boundFor")]
    public string? BoundFor;

    [JsonProperty("remainingDistance")]
    public float? RemainingDistance;

    [JsonProperty("serviceType")]
    public string? ServiceType;

    [JsonProperty("stations")]
    public List<Station>? Stations;
}
