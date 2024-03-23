using Newtonsoft.Json;

namespace OpenTetsu.Commons.Train;

public class Lamps
{
    [JsonProperty("ats")]
    public LampsAts? Ats;

    [JsonProperty("eBrake")]
    public bool EBrake;

    [JsonProperty("ebTimer")]
    public bool EbTimer;

    [JsonProperty("overload")]
    public bool Overload;

    [JsonProperty("pilot")]
    public bool Pilot;

    [JsonProperty("regenBrake")]
    public bool RegenBrake;
}