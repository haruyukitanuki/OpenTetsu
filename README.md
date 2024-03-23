# OpenTetsu API Standard
OpenTetsu is an open source API standard for Japanese train simulators and third-party software/plugins to communicate with each other harmoniously.

Originally created to support development of Tanuden TIMS for TRAIN CREW.

## 📖 Why create a standard?
Handling and supporting data structures for multiple platforms and train simulator software can be hard and sometimes even tedious to do so.

**OpenTetsu was designed with the following goals:**
- Platform & software agnostic
- Unify data structures between simulator software
- Data transmission over REST API (using JSON)
- Sensible and logical data structure

## 🔌 Adapter
As OpenTetsu is just an API standard, you will need to use/write an adapter in order to translate the raw simulator data to OpenTetsu API.

**Current list of adapters:**
- TRAIN CREW

> [!NOTE]
> As of time of writing this, the only available adapter for OpenTetsu API is for TRAIN CREW. 
> 
> If you wish to write an adapter for other simulators, please open a pull request to contribute to our list of adapters!

## 📂 Installation
To install OpenTetsu into your project:
1. Go to the releases section to download the latest OpenTetsu API library DLL file.
2. In the same section, find and download the appropriate adapter for the simulator you are developing for.
3. Reference OpenTetsu in your project.
   - Need help referencing? View docs for [Visual Studio](https://learn.microsoft.com/en-us/visualstudio/ide/how-to-add-or-remove-references-by-using-the-reference-manager?view=vs-2022) or [JetBrains Rider](https://www.jetbrains.com/help/rider/Extending_Your_Solution.html#project_assembly_references).
4. Install required dependency `Newtonsoft.Json` from NuGet.
5. All good to go!

## ⌨️ Usage
### With adapter
This example is written using `OpenTetsu.Adapters.TrainCrewAdapter` for usage on TRAIN CREW.

```cs
using TrainCrew; // Library provided by the simulator
using OpenTetsu.Commons;
using OpenTetsu.Adapters;

public static class Program
{
    public static void Main
    {
        // Get Raw data from the simulator
        TrainCrewInput.Init();
        var trainCrewState = TrainCrewInput.GetTrainState();
        TrainCrewInput.RequestData(DataRequest.Signal);
        var trainCrewSignalList = TrainCrewInput.signals;

        // Convert to OpenTetsu
        OpenTetsuData convertedData = TrainCrewAdapter.FromTrainCrew(trainCrewState, trainCrewSignalList)

        ...
    } 
}

```

### Without adapter - Bring your own adapter
Using OpenTetsu API without an adapter requires you to populate OpenTetsuData classes manually. 

You can view the list of classes below and reference the code of an existing adapter to write your own adapter (Existing adapters can be found in `OpenTetsu.Adapters` in this repository).

<details>
<summary>List of OpenTetsuData Classes</summary>

* **OpenTetsu.Commons**
  * **Ats**
    * AtsState
  * Controller
    * ControllerState
  * **Route**
    * Direction
    * NextStation
    * Diagram
    * Station
    * StationTimings
    * StopType
  * **SignalState**
    * Signal
    * SignalType
    * Transponder
  * **Train**
    * CarProperties
    * CarState
    * Lamps
    * LampsAts
    * NextSpeedLimit
    * SpeedLimitType
    * TrainState
</details>

## Sample Data
```json
{
  "runNumber": "573",
  "currentTime": "2024-03-23T05:19:34.233+08:00",
  "diagram": {
    "direction": "Outbound",
    "boundFor": "館浜",
    "remainingDistance": 9690.928,
    "serviceType": "普通",
    "stations": [
      {
        "distanceFromKmZero": 0,
        "index": 0,
        "name": "浜園上り本線",
        "positionName": "浜園駅入換下り",
        "stopType": "OperationStop",
        "timings": {
          "arrival": "2024-03-23T05:16:10+08:00",
          "departure": "2024-03-23T05:19:30+08:00"
        }
      },
      {
        "distanceFromKmZero": 192.4,
        "index": 1,
        "name": "浜園",
        "positionName": "浜園駅下り",
        "stopType": "PassengerStop",
        "timings": {
          "arrival": "2024-03-23T05:20:15+08:00",
          "departure": "2024-03-23T05:21:05+08:00"
        }
      },
      {
        "distanceFromKmZero": 1722.6,
        "index": 2,
        "name": "津崎",
        "positionName": "津崎駅3番下り",
        "stopType": "PassengerStop",
        "timings": {
          "arrival": "2024-03-23T05:22:45+08:00",
          "departure": "2024-03-23T05:23:10+08:00"
        }
      },
      {
        "distanceFromKmZero": 3698.2,
        "index": 3,
        "name": "虹ケ浜",
        "positionName": "虹ケ浜駅下り",
        "stopType": "PassengerStop",
        "timings": {
          "arrival": "2024-03-23T05:25:05+08:00",
          "departure": "2024-03-23T05:25:25+08:00"
        }
      },
      {
        "distanceFromKmZero": 5638.4,
        "index": 4,
        "name": "海岸公園",
        "positionName": "海岸公園駅下り",
        "stopType": "PassengerStop",
        "timings": {
          "arrival": "2024-03-23T05:27:20+08:00",
          "departure": "2024-03-23T05:27:40+08:00"
        }
      },
      {
        "distanceFromKmZero": 6958.6,
        "index": 5,
        "name": "河原崎",
        "positionName": "河原崎駅下り",
        "stopType": "PassengerStop",
        "timings": {
          "arrival": "2024-03-23T05:29:15+08:00",
          "departure": "2024-03-23T05:29:35+08:00"
        }
      },
      {
        "distanceFromKmZero": 7990.6,
        "index": 6,
        "name": "駒野",
        "positionName": "駒野駅3番下り",
        "stopType": "PassengerStop",
        "timings": {
          "arrival": "2024-03-23T05:31:00+08:00",
          "departure": "2024-03-23T05:31:30+08:00"
        }
      },
      {
        "distanceFromKmZero": 9703.6,
        "index": 7,
        "name": "館浜",
        "positionName": "館浜駅4番下り",
        "stopType": "PassengerStop",
        "timings": {
          "arrival": "2024-03-23T05:34:55+08:00",
          "departure": "2024-03-23T05:35:25+08:00"
        }
      }
    ]
  },
  "nextStation": {
    "distanceFromTrain": 179.732,
    "distanceFromKmZero": 192.4,
    "index": 1,
    "name": "浜園",
    "positionName": "浜園駅下り",
    "stopType": "PassengerStop",
    "timings": {
      "arrival": "2024-03-23T05:20:15+08:00",
      "departure": "2024-03-23T05:21:05+08:00"
    }
  },
  "trainState": {
    "carStates": [
      {
        "amperage": 482.8885,
        "bcPressure": 0,
        "carNo": 1,
        "isDoorClosed": true,
        "model": "5300",
        "properties": {
          "pantograph": false,
          "driverCab": true,
          "conductorCab": true,
          "motor": true,
          "cabDirection": "Outbound"
        }
      },
      {
        "amperage": 0,
        "bcPressure": 0,
        "carNo": 2,
        "isDoorClosed": true,
        "model": "5300",
        "properties": {
          "pantograph": true,
          "driverCab": false,
          "conductorCab": false,
          "motor": false,
          "cabDirection": null
        }
      },
      {
        "amperage": 0,
        "bcPressure": 0,
        "carNo": 3,
        "isDoorClosed": true,
        "model": "5300",
        "properties": {
          "pantograph": false,
          "driverCab": false,
          "conductorCab": false,
          "motor": false,
          "cabDirection": null
        }
      },
      {
        "amperage": 482.8885,
        "bcPressure": 0,
        "carNo": 4,
        "isDoorClosed": true,
        "model": "5300",
        "properties": {
          "pantograph": false,
          "driverCab": true,
          "conductorCab": true,
          "motor": true,
          "cabDirection": "Inbound"
        }
      }
    ],
    "consist": 4,
    "lamps": {
      "ats": {
        "brakeApplication": false,
        "inOperation": true,
        "isolated": false
      },
      "eBrake": false,
      "ebTimer": false,
      "overload": false,
      "pilot": true,
      "regenBrake": false
    },
    "mrPressure": 700,
    "nextSpeedLimit": {
      "distance": -1,
      "limit": -1,
      "type": "SpeedLimit"
    },
    "speed": 15.51307,
    "speedLimit": 25,
    "speedLimitType": "Signal",
    "gradient": -3,
    "distanceFromKmZero": 12.672
  },
  "signalStates": [],
  "atsState": {
    "stopPattern": null,
    "speed": 30,
    "state": "無表示"
  },
  "controllerState": {
    "notch": 3,
    "reverser": 1
  }
}
```

## 💾 Tanuden OSS
OpenTetsu is Open Source Software (OSS), licensed under Mozilla Public License 2.0. You may freely distribute, use and modify code provided to you in repository in accordance with MPL-2.0.

A copy of the license can be found at the root of the repository [here](https://github.com/haruyukitanuki/OpenTetsu/blob/main/LICENSE.md).

## 💝 Support
[Tanuden Discord Server](https://go.tanu.ch/tanuden-discord) | [Twitter](https://go.tanu.ch/twitter) | [YouTube](https://go.tanu.ch/tanutube)

**Tanukigawa Electric Railway | Copyright (c) 2024 Haruyuki Tanukiji.**