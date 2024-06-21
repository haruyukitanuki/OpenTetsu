using Newtonsoft.Json;

namespace OpenTetsu.Commons.Controller;

public class ControllerState
{
    [JsonProperty("notch")]
    public int Notch;

    [JsonProperty("bNotch")] 
    public int BNotch;
    
    [JsonProperty("pNotch")] 
    public int PNotch;

    [JsonProperty("reverser")]
    public int Reverser;
}
