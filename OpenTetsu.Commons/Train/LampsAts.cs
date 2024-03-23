using Newtonsoft.Json;

namespace OpenTetsu.Commons.Train;

public class LampsAts
{
    [JsonProperty("brakeApplication")]
    public bool BrakeApplication;

    [JsonProperty("inOperation")]
    public bool InOperation;

    [JsonProperty("isolated")]
    public bool Isolated;
}