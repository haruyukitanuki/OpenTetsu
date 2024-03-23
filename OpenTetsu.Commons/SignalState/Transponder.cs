using Newtonsoft.Json;

namespace OpenTetsu.Commons.Signal;

public class Transponder
{
    [JsonProperty("type")]
    public string? Type;
        
    [JsonProperty("speedlimit")]
    public float SpeedLimit;
        
    [JsonProperty("distance")]
    public float Distance;
}