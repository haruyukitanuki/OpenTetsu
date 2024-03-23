using Newtonsoft.Json;
using OpenTetsu.Commons.Route;

namespace OpenTetsu.Commons.Train;

public class CarProperties
{
    [JsonProperty("pantograph")]
    public bool Pantograph;
        
    [JsonProperty("driverCab")]
    public bool DriverCab;
        
    [JsonProperty("conductorCab")]
    public bool ConductorCab;
        
    [JsonProperty("motor")]
    public bool Motor;
        
    [JsonProperty("cabDirection")]
    public Direction? CabDirection;
}