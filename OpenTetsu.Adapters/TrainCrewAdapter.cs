using OpenTetsu.Commons;
using OpenTetsu.Commons.ATS;
using OpenTetsu.Commons.Controller;
using OpenTetsu.Commons.Route;
using OpenTetsu.Commons.SignalState;
using OpenTetsu.Commons.Train;
using TrainCrew;
using CarState = OpenTetsu.Commons.Train.CarState;
using Station = OpenTetsu.Commons.Route.Station;
using StopType = OpenTetsu.Commons.Route.StopType;
using TrainState = OpenTetsu.Commons.Train.TrainState;

namespace OpenTetsu.Adapters;

public static class TrainCrewAdapter
{
    public static OpenTetsuData FromTrainCrew(TrainCrew.TrainState rawTrainState, List<TrainCrew.SignalInfo>? signals)
    {
        // Format signal data
        List<SignalState> formattedSignalData = new();

        SignalType DetermineSignalType(string name)
        {
            if (name.Contains("入換")) return SignalType.Switch;
            if (name.Contains("場内")) return SignalType.Home;
            if (name.Contains("出発")) return SignalType.Departure;

            return SignalType.Standard;
        }

        if (signals != null)
            formattedSignalData = signals.Select(signal => new SignalState
            {
                Name = signal.name,
                Type = DetermineSignalType(signal.name),
                Phase = signal.phase,
                Distance = signal.distance,
                Transponders = signal.beacons.Select(transponder => new Transponder
                {
                    Distance = transponder.distance,
                    SpeedLimit = transponder.speed,
                    Type = transponder.type
                }).ToList()
            }).ToList();

        SpeedLimitType DetermineSpeedLimitType(float speedLimit, float? distance = null)
        {
            // Determine without distance -- Lacks accuracy
            if (distance == null) return speedLimit is 0 or 25 or 55 or 80 ? SpeedLimitType.Signal : SpeedLimitType.SpeedLimit;

            var type = SpeedLimitType.SpeedLimit;
            // For floating point comparison
            var tolerance = 1;

            formattedSignalData.ForEach(signal =>
            {
                // Find a signal that has the same distance.
                signal.Transponders!.ForEach(transponder =>
                {
                    if (Math.Abs(transponder.Distance - (float)distance) > tolerance) return;

                    type = SpeedLimitType.Signal;
                });
            });

            return type;
        }
        
        StopType DetermineStopType(TrainCrew.StopType stopType)
        {
            return stopType switch
            {
                TrainCrew.StopType.StopForPassenger => StopType.PassengerStop,
                TrainCrew.StopType.StopForOperation => StopType.OperationStop,
                TrainCrew.StopType.Passing => StopType.Passing,
                _ => throw new ArgumentOutOfRangeException(nameof(stopType), stopType, null)
            };
        }

        // Find which direction towards
        // （下り）<<<<< 館浜 | 大路 >>>>>（上り）
        // Count the number of 上　and 下
        var upCount = rawTrainState.stationList.Count(station => station.StopPosName.Contains('上'));
        var downCount = rawTrainState.stationList.Count(station => station.StopPosName.Contains('下'));

        // If 上 is more than 下, then it's bound for 上
        var direction = upCount > downCount ? Direction.Inbound : Direction.Outbound;

        // Convert (timespan)rawTrainState.NowTime to DateTime
        // NowTime is a Timespan. Set the date to today.
        var relativeNowTime = DateTime.Today.Add(rawTrainState.NowTime);
        
        // Order the cars according to direction. 
        // For some reason the first car in CarStates is facing Otebashi and the last car is facing Tatehama.
        // This is wrong. I don't know why. But let's just reverse it.
        var carStates = rawTrainState.CarStates;
        if (direction == Direction.Inbound) carStates.Reverse();
        
        // Populate NextStation
        // rawTrainState.stationList.Select((station, index) => 
        NextStation? nextStation = null;
        if (rawTrainState.stationList.Count != 0 && rawTrainState.nowStaIndex <= rawTrainState.stationList.Count - 1)
        {
            var nextStationInfo = rawTrainState.stationList[rawTrainState.nowStaIndex];
            nextStation = new NextStation
            {
                Name = nextStationInfo.Name,
                Index = rawTrainState.nowStaIndex,
                Timings = new StationTimings
                {
                    Arrival = DateTime.Today.Add(nextStationInfo.ArvTime),
                    Departure = DateTime.Today.Add(nextStationInfo.DepTime)
                },
                StopType = DetermineStopType(nextStationInfo.stopType),
                PositionName = nextStationInfo.StopPosName,
                DistanceFromKmZero = nextStationInfo.TotalLength,
                DistanceFromTrain = rawTrainState.nextStaDistance
            };
        }

        // Process carStates
        // Pass 1: Format into OpenTetsu without CabDirection
        var carStatesPass1 = carStates.Select((carState, index) => new CarState
        {
            CarNo = index + 1,
            IsDoorClosed = carState.DoorClose,
            BcPressure = carState.BC_Press,
            Amperage = carState.Ampare,
            Model = carState.CarModel,
            Properties = new CarProperties
            {
                Pantograph = carState.HasPantograph,
                DriverCab = carState.HasDriverCab,
                ConductorCab = carState.HasConductorCab,
                Motor = carState.HasMotor
            }
        }).ToList();
        
        //　Pass 2: Determine CabDirection
        var carStatesPass2 = carStatesPass1.Select((carState, index) =>
        {
            if (!carState.Properties!.ConductorCab) return carState;
            
            var isPreviousCarHasDriverCab = index != 0 && carStatesPass1[index - 1].Properties!.DriverCab;
            var isCurrentCarHasDriverCab = carState.Properties!.DriverCab;
            
            // First and last car
            if (index == 0)
            {
                carState.Properties.CabDirection = Direction.Outbound;
                return carState;
            }
            if (index == carStatesPass1.Count - 1)
            {
                carState.Properties.CabDirection = Direction.Inbound;
                return carState;
            }
            
            // Middle cars
            if (isCurrentCarHasDriverCab)
            {
                carState.Properties.CabDirection = Direction.Outbound;
            }
            
            return carState;
        }).ToList();
        
        // Pass 3: Flip any car that has DriverCab and is facing the wrong direction
        var carStatesPass3 = carStatesPass2.Select((carState, index) =>
        {
            // Ignore first and last
            if (index == 0 || index == carStatesPass2.Count - 1) return carState;
            
            // If all cars has driving cabs, alternate between Inbound and Outbound
            
            var previousCar = carStatesPass2[index - 1];
            var isPreviousCarHasDriverCab = index != 0 && carStatesPass1[index - 1].Properties!.DriverCab;

            if (previousCar.Properties.CabDirection == carState.Properties.CabDirection)
            {
                carState.Properties.CabDirection = carState.Properties.CabDirection == Direction.Inbound ? Direction.Outbound : Direction.Inbound;
            }
            
            if (!isPreviousCarHasDriverCab && carState.Properties.DriverCab)
            {
                carState.Properties.CabDirection = Direction.Inbound;
            }
            
            return carState;
        }).ToList();


        var formattedData = new OpenTetsuData
        {
            RunNumber = rawTrainState.diaName,
            CurrentTime = relativeNowTime,
            NextStation = nextStation,
            TrainState = new TrainState
            {
                Consist = rawTrainState.CarStates.Count,
                Speed = rawTrainState.Speed,
                SpeedLimit = rawTrainState.speedLimit,
                SpeedLimitType = DetermineSpeedLimitType(rawTrainState.speedLimit),
                Gradient = rawTrainState.gradient,
                NextSpeedLimit = new NextSpeedLimit
                {
                    Limit = rawTrainState.nextSpeedLimit,
                    Type = DetermineSpeedLimitType(rawTrainState.nextSpeedLimit, rawTrainState.nextSpeedLimitDistance),
                    Distance = rawTrainState.nextSpeedLimitDistance
                },
                MrPressure = rawTrainState.MR_Press, 
                DistanceFromKmZero = rawTrainState.TotalLength,
                CarStates =  carStatesPass3,

                Lamps = new Lamps
                {
                    Pilot = rawTrainState.Lamps[PanelLamp.DoorClose],
                    RegenBrake = rawTrainState.Lamps[PanelLamp.RegenerativeBrake],
                    EBrake = rawTrainState.Lamps[PanelLamp.EmagencyBrake],
                    EbTimer = rawTrainState.Lamps[PanelLamp.EB_Timer],
                    Overload = rawTrainState.Lamps[PanelLamp.Overload],

                    Ats = new LampsAts
                    {
                        InOperation = rawTrainState.Lamps[PanelLamp.ATS_Ready],
                        BrakeApplication = rawTrainState.Lamps[PanelLamp.ATS_BrakeApply],
                        Isolated = rawTrainState.Lamps[PanelLamp.ATS_Open]
                    }
                }
            },

            SignalStates = formattedSignalData,

            ControllerState = new ControllerState
            {
                Notch = rawTrainState.Pnotch - rawTrainState.Bnotch,
                Reverser = rawTrainState.Reverser
            },

            AtsState = new AtsState
            {
                StopPattern = rawTrainState.ATS_Class,
                Speed = float.Parse(rawTrainState.ATS_Speed),
                State = rawTrainState.ATS_State
            },

            Diagram = new Diagram
            {
                Direction = direction,
                BoundFor = rawTrainState.BoundFor,
                ServiceType = rawTrainState.Class,

                // Take the last item on rawData.stations and subtract with rawData.TotalLength
                RemainingDistance = rawTrainState.stationList.Count > 0
                    ? rawTrainState.stationList.Last().TotalLength - rawTrainState.TotalLength
                    : 0,

                Stations = rawTrainState.stationList.Select((station, index) => new Station
                {
                    Name = station.Name,
                    Index = index,
                    Timings = new StationTimings
                    {
                        Arrival = DateTime.Today.Add(station.ArvTime),
                        Departure = DateTime.Today.Add(station.DepTime)
                    },
                    StopType = DetermineStopType(station.stopType),
                    PositionName = station.StopPosName,
                    DistanceFromKmZero = station.TotalLength
                }).ToList()
            }
        };

        return formattedData;
    }
}