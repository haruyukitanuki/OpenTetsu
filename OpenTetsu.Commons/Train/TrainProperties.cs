using Newtonsoft.Json;
using OpenTetsu.Commons.Route;

namespace OpenTetsu.Commons.Train;

public class TrainProperties
{
    [JsonProperty("model")]
    public string? Model;
    
    [JsonProperty("holdBrake")]
    public bool HoldBrake;

    [JsonProperty("pantographType")] 
    public PantographType? PantographType;
    
    [JsonProperty("pantographDirections")]
    public Dictionary<int, Direction?[]>? PantographDirections;
}
