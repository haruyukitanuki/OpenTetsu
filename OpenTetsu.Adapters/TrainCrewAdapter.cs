using System.Collections.ObjectModel;
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
    public static OpenTetsuData FromTrainCrew(TrainCrew.TrainState rawTrainState, List<SignalInfo>? signals, TrainSwitch switches, DateTime currentDate)
    {
        // Format signal data
        List<SignalState> formattedSignalData = new();

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

        // Find which direction towards
        // （下り）<<<<< 館浜 | 大路 >>>>>（上り）
        // If Diagram number is even number, it's Inbound. If it's odd, it's Outbound.

        // Remove all letters from diaName
        var diagramNumber = new string(rawTrainState.diaName.Where(char.IsDigit).ToArray());
        if (diagramNumber == string.Empty) diagramNumber = "0";
        var direction = diagramNumber[^1] % 2 == 0 ? Direction.Inbound : Direction.Outbound;

        // Convert (timespan)rawTrainState.NowTime to DateTime
        // NowTime is a Timespan. Set it into Timespan with JST timezone
        var relativeNowTime = currentDate.Add(rawTrainState.NowTime);
        relativeNowTime = DateTime.SpecifyKind(relativeNowTime, DateTimeKind.Unspecified);


        // Order the cars according to direction. 
        // For some reason the first car in CarStates is facing Otebashi and the last car is facing Tatehama.
        // This is wrong. I don't know why. But let's just reverse it.
        var rawCarStates = rawTrainState.CarStates;
        if (direction == Direction.Inbound) rawCarStates.Reverse();

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
                    Arrival = DateTime.SpecifyKind(currentDate.Add(nextStationInfo.ArvTime),
                        DateTimeKind.Unspecified),
                    Departure = DateTime.SpecifyKind(currentDate.Add(nextStationInfo.DepTime),
                        DateTimeKind.Unspecified)
                },
                DoorDirection = nextStationInfo.doorDir == DoorDir.LeftSide
                    ? DoorDirection.LeftSide
                    : DoorDirection.RightSide,
                StopType = DetermineStopType(nextStationInfo.stopType),
                PositionName = nextStationInfo.StopPosName,
                Distance = nextStationInfo.TotalLength,
                DistanceFromTrain = rawTrainState.nextStaDistance
            };
        }

        var formattedData = new OpenTetsuData
        {
            RunNumber = DetermineRunNumber(rawTrainState.diaName),
            DiagramNumber = rawTrainState.diaName,
            CurrentTime = relativeNowTime,
            NextStation = nextStation,
            Train = new TrainState
            {
                Consist = rawTrainState.CarStates.Count,
                Speed = rawTrainState.Speed,
                SpeedLimit = rawTrainState.speedLimit,
                SpeedLimitType = DetermineSpeedLimitType(rawTrainState.speedLimit),
                Gradient = rawTrainState.gradient,
                NextSpeedLimit = new NextSpeedLimit
                {
                    Limit = rawTrainState.nextSpeedLimit,
                    Type =   DetermineSpeedLimitType(rawTrainState.nextSpeedLimit, rawTrainState.nextSpeedLimitDistance),
                    Distance = rawTrainState.nextSpeedLimitDistance
                },
                MrPressure = rawTrainState.MR_Press,
                Distance = rawTrainState.TotalLength,
                Cars = DetermineCabDirection(rawCarStates),
                
                Switches = new Switches
                {
                    Horn_Air = switches.Horn_Air,
                    Horn_Electric = switches.Horn_Electric,
                    BuzzerM = switches.buzzerM,
                    BuzzerC = switches.buzzerC,
                    BuzzerAuto = switches.buzzerAuto,
                    HighBeam = switches.highBeam,
                },

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

            Signals = formattedSignalData,

            ControllerState = new ControllerState
            {
                PNotch = rawTrainState.Pnotch,
                BNotch = rawTrainState.Bnotch,
                Notch = rawTrainState.Pnotch - rawTrainState.Bnotch,
                Reverser = rawTrainState.Reverser
            },

            Ats = new AtsState
            {
                StopPattern = rawTrainState.ATS_Class,
                Speed = float.Parse(rawTrainState.ATS_Speed == "F" ? "300" : rawTrainState.ATS_Speed),
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

                Stations = rawTrainState.stationList.Select((station, index) =>
                {
                    var relativeArrivalTime = currentDate.Add(station.ArvTime);
                    var relativeDepartureTime = currentDate.Add(station.DepTime);

                    return new Station
                    {
                        Name = station.Name,
                        Index = index,
                        Timings = new StationTimings
                        {
                            Arrival = DateTime.SpecifyKind(relativeArrivalTime, DateTimeKind.Unspecified),
                            Departure = DateTime.SpecifyKind(relativeDepartureTime, DateTimeKind.Unspecified)
                        },
                        DoorDirection = station.doorDir == DoorDir.LeftSide ? DoorDirection.LeftSide : DoorDirection.RightSide,
                        StopType = DetermineStopType(station.stopType),
                        PositionName = station.StopPosName,
                        Distance = station.TotalLength
                    };
                }).ToList()
            }
        };

        return formattedData;

        SpeedLimitType DetermineSpeedLimitType(float speedLimit, float? distance = null)
        {
            // Determine without distance -- Lacks accuracy
            if (distance == null)
                return speedLimit is 0 or 25 or 55 or 80 ? SpeedLimitType.Signal : SpeedLimitType.SpeedLimit;

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

        SignalType DetermineSignalType(string name)
        {
            if (name.Contains("入換")) return SignalType.Switch;
            if (name.Contains("場内")) return SignalType.Home;
            if (name.Contains("出発")) return SignalType.Departure;

            return SignalType.Standard;
        }

        List<CarState> DetermineCabDirection(List<TrainCrew.CarState> rawCars)
        {
            // Process carStates
            // Pass 1: Format into OpenTetsu without CabDirection
            var carStatesPass1 = rawCars.Select((carState, index) => new CarState
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
                    CabDirection = null,
                    ConductorCab = carState.HasConductorCab,
                    Motor = carState.HasMotor
                }
            }).ToList();

            //　Pass 2: Determine CabDirection
            var carStatesPass2 = carStatesPass1.Select((carState, index) =>
            {
                if (!carState.Properties!.DriverCab) return carState;
                
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
                if (!carState.Properties!.DriverCab) return carState;
                
                // Ignore first and last
                if (index == 0 || index == carStatesPass2.Count - 1) return carState;

                // If all cars have driving cabs, alternate between Inbound and Outbound

                var previousCar = carStatesPass2[index - 1];
                var isPreviousCarHasDriverCab = carStatesPass1[index - 1].Properties!.DriverCab;

                if (previousCar.Properties!.CabDirection == carState.Properties!.CabDirection)
                {
                    carState.Properties.CabDirection = carState.Properties.CabDirection == Direction.Inbound
                        ? Direction.Outbound
                        : Direction.Inbound;
                }

                if (!isPreviousCarHasDriverCab && carState.Properties.DriverCab)
                {
                    carState.Properties.CabDirection = Direction.Inbound;
                }

                return carState;
            }).ToList();
            
            return carStatesPass3;
        }
        
        string? DetermineRunNumber(String diaName)
        {
            // [数字抽出]
            // if over3000 +100
            // if over 6000 +200
            //
            //     [下2桁抽出]
            // if 奇数 -1
            // ありがとう、ゐづるさん

            if (diaName == String.Empty) return null;

            var runNumber = diaName;

            // If the last character is a letter, remove it
            if (char.IsLetter(runNumber[^1])) runNumber = runNumber.Substring(0, runNumber.Length - 1);

            // If the last character is a letter again, remove it. In case the diagram number is like 9999AX
            if (char.IsLetter(runNumber[^1])) runNumber = runNumber.Substring(0, runNumber.Length - 1);

            // If the first character is not a number, remove it (For test run and kaisou)
            if (!char.IsDigit(runNumber[0]))
            {
                runNumber = runNumber[1..];
            }

            var lastTwoDigits = runNumber.Substring(runNumber.Length - 2);

            var runNumberInt = int.Parse(lastTwoDigits);

            switch (int.Parse(runNumber))
            {
                case >= 6000:
                    runNumberInt += 200;
                    break;
                case >= 3000:
                    runNumberInt += 100;
                    break;
            }

            if (runNumberInt % 2 != 0) runNumberInt -= 1;

            runNumber = runNumberInt.ToString();
            return runNumber;
        }
    }
}