using Newtonsoft.Json;

namespace OpenTetsu.Commons.Signal;

public class SignalState
{
    [JsonProperty("name")]
    public string? Name;
        
    [JsonProperty("type")]
    public SignalType Type;
        
    [JsonProperty("phase")]
    public string? Phase;
        
    [JsonProperty("distance")]
    public float Distance;
        
    [JsonProperty("transponders")]
    public List<Transponder>? Transponders;
}