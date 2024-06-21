using Newtonsoft.Json;
using OpenTetsu.Commons.ATS;
using OpenTetsu.Commons.Controller;
using OpenTetsu.Commons.Route;
using OpenTetsu.Commons.Train;

namespace OpenTetsu.Commons;

public class OpenTetsuData
{
    [JsonProperty("runNumber")] public string? RunNumber; // 運行番号
    
    [JsonProperty("diagramNumber")] public string? DiagramNumber; // 列車番号
    
    [JsonProperty("currentTime")] public DateTime? CurrentTime;
    
    [JsonProperty("diagram")] public Diagram? Diagram;
    
    [JsonProperty("nextStation")] public NextStation? NextStation;
    
    [JsonProperty("trainState")] public TrainState? TrainState;
    
    [JsonProperty("signalStates")] public List<SignalState.SignalState>? SignalStates;
    
    [JsonProperty("atsState")] public AtsState? AtsState;

    [JsonProperty("controllerState")] public ControllerState? ControllerState;
}