using Newtonsoft.Json;

namespace OpenTetsu.Commons.Train;

public class CarState
{
    [JsonProperty("amperage")]
    public float Amperage;

    [JsonProperty("bcPressure")]
    public float BcPressure;

    [JsonProperty("carNo")]
    public int CarNo;

    [JsonProperty("isDoorClosed")]
    public bool IsDoorClosed;

    [JsonProperty("model")] 
    public string? Model;
        
    [JsonProperty("properties")]
    public CarProperties? Properties;
}