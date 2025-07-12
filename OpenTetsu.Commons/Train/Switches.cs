using Newtonsoft.Json;

namespace OpenTetsu.Commons.Train;

public class Switches
{
    [JsonProperty("hornAir")]
    public bool Horn_Air;
    
    [JsonProperty("hornElectric")]
    public bool Horn_Electric;
    
    [JsonProperty("buzzerM")]
    public bool BuzzerM;
    
    [JsonProperty("buzzerC")]
    public bool BuzzerC;
    
    [JsonProperty("buzzerAuto")]
    public bool BuzzerAuto;
    
    [JsonProperty("highBeam")]
    public bool HighBeam;
}