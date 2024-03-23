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

## 💾 Tanuden OSS
OpenTetsu is Open Source Software (OSS), licensed under Mozilla Public License 2.0. You may freely distribute, use and modify code provided to you in repository in accordance with MPL-2.0.

A copy of the license can be found at the root of the repository [here](https://github.com/haruyukitanuki/OpenTetsu/blob/main/LICENSE.md).

## 💝 Support
[Tanuden Discord Server](https://go.tanu.ch/tanuden-discord) | [Twitter](https://go.tanu.ch/twitter) | [YouTube](https://go.tanu.ch/tanutube)

**Tanukigawa Electric Railway | Copyright (c) 2024 Haruyuki Tanukiji.**