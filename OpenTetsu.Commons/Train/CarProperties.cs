using Newtonsoft.Json;
using OpenTetsu.Commons.Route;

namespace OpenTetsu.Commons.Train;

public class CarProperties
{
    [JsonProperty("driverCab")]
    public bool DriverCab;
        
    [JsonProperty("conductorCab")]
    public bool ConductorCab;
        
    [JsonProperty("motor")]
    public bool Motor;
        
    [JsonProperty("cabDirection")]
    public Direction? CabDirection;
    
    [JsonProperty("pantograph")]
    public bool Pantograph;
    
    [JsonProperty("pantographDirection")]
    public Direction? PantographDirection;

    [JsonProperty("pantographType")] 
    public PantographType? PantographType;
}
