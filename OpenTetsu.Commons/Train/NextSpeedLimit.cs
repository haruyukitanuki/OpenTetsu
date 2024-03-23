using Newtonsoft.Json;

namespace OpenTetsu.Commons.Train;

public class NextSpeedLimit
{
    [JsonProperty("distance")]
    public float Distance;

    [JsonProperty("limit")]
    public float Limit;

    [JsonProperty("type")]
    public SpeedLimitType? Type;
}