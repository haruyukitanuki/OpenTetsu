using Newtonsoft.Json;

namespace OpenTetsu.Commons.ATS;

public class AtsState
{
    [JsonProperty("stopPattern")]
    public string? StopPattern;

    [JsonProperty("speed")]
    public float? Speed;

    [JsonProperty("state")]
    public string? State;
}