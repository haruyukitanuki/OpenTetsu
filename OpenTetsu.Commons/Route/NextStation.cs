using Newtonsoft.Json;

namespace OpenTetsu.Commons.Route;

public class NextStation : Station
{
    [JsonProperty("distanceFromTrain")]
    public float? DistanceFromTrain;
}