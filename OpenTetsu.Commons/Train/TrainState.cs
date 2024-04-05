using Newtonsoft.Json;

namespace OpenTetsu.Commons.Train;

public class TrainState
{
    [JsonProperty("carStates")]
    public List<CarState>? CarStates;

    [JsonProperty("consist")]
    public int Consist;

    [JsonProperty("lamps")]
    public Lamps? Lamps;

    [JsonProperty("mrPressure")]
    public float MrPressure;

    [JsonProperty("nextSpeedLimit")]
    public NextSpeedLimit? NextSpeedLimit;

    [JsonProperty("speed")]
    public float Speed;

    [JsonProperty("speedLimit")]
    public float SpeedLimit;

    [JsonProperty("speedLimitType")]
    public SpeedLimitType? SpeedLimitType;
        
    [JsonProperty("gradient")]
    public float Gradient;
        
    [JsonProperty("distanceFromKmZero")]
    public float DistanceFromKmZero;
}
