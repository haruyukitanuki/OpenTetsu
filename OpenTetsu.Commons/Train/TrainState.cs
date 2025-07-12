using Newtonsoft.Json;

namespace OpenTetsu.Commons.Train;

public class TrainState
{
    [JsonProperty("cars")]
    public List<CarState>? Cars;

    [JsonProperty("consist")]
    public int Consist;

    [JsonProperty("lamps")]
    public Lamps? Lamps;

    [JsonProperty("mrPressure")]
    public float MrPressure;

    [JsonProperty("speed")]
    public float Speed;

    [JsonProperty("speedLimit")]
    public float SpeedLimit;

    [JsonProperty("speedLimitType")]
    public SpeedLimitType? SpeedLimitType;

    [JsonProperty("nextSpeedLimit")]
    public NextSpeedLimit? NextSpeedLimit;

    [JsonProperty("gradient")]
    public float Gradient;

    [JsonProperty("distance")]
    public float Distance;
    
    [JsonProperty("switches")]
    public Switches? Switches;
}
