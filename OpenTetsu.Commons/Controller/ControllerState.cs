using Newtonsoft.Json;

namespace OpenTetsu.Commons.Controller;

public class ControllerState
{
    [JsonProperty("notch")]
    public int Notch;

    [JsonProperty("reverser")]
    public int Reverser;
}
