using Newtonsoft.Json;
using OpenTetsu.Commons.Ats;
using OpenTetsu.Commons.Controller;
using OpenTetsu.Commons.Route;
using OpenTetsu.Commons.Signal;

namespace OpenTetsu.Commons;

public class OpenTetsuData
{
    [JsonProperty("runNumber")] public string? RunNumber;
    
    [JsonProperty("currentTime")] public DateTime? CurrentTime;
    
    [JsonProperty("diagram")] public Diagram? Diagram;
    
    [JsonProperty("nextStation")] public NextStation? NextStation;
    
    [JsonProperty("trainState")] public TrainState? TrainState;
    
    [JsonProperty("signalStates")] public List<SignalState>? SignalStates;
    
    [JsonProperty("atsState")] public AtsState? AtsState;

    [JsonProperty("controllerState")] public ControllerState? ControllerState;
}