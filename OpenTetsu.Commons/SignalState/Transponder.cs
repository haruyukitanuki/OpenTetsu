using Newtonsoft.Json;

namespace OpenTetsu.Commons.SignalState;

public class Transponder
{
    [JsonProperty("type")]
    public string? Type;
        
    [JsonProperty("speedlimit")]
    public float SpeedLimit;
        
    [JsonProperty("distance")]
    public float Distance;
}