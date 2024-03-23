# OpenTetsu API 標準
OpenTetsu は、日本の電車シミュレータとサードパーティのソフトウェア・プラグインが円滑にコミュニケーションを取るためのオープンソース API 標準です。

タヌ電TIMSの開発をサポートするために元々作成されました。

> [!TIP]
> このドキュメントは英語版と日本語版があります。<br>
> This documentation is available in English & Japanese
> 
> [![lang - en](https://img.shields.io/static/v1?label=lang&message=en&color=397eed)](https://github.com/haruyukitanuki/OpenTetsu/blob/main/README.md) 
> [![言語 - jp](https://img.shields.io/static/v1?label=言語&message=jp&color=e32b47)](https://github.com/haruyukitanuki/OpenTetsu/blob/main/README-jp.md)

## 📖 なぜ標準を作成するのですか？

複数のプラットフォームや電車シミュレータソフトウェア向けのデータ構造を扱うことは、難しく、時には煩雑になることがあります。

**OpenTetsuの目標は**

*   プラットフォームやソフトウェアに依存しない
*   シミュレータソフトウェア間のデータ構造を統一する
*   REST API を介したデータの送受信（JSONで）
*   賢明で論理的なデータ構造

## 🔌 アダプター（Adapter）
OpenTetsu は単なる API 標準であり、シミュレータデータを OpenTetsu API に変換するためにアダプターを使用する必要があります。

**現在のアダプターのリスト:**
* TRAIN CREW

> [!NOTE] 
> このドキュメントを作成した時点では、OpenTetsu API の唯一の利用可能なアダプターは TRAIN CREW 用です。
> 
> 他のシミュレーター用のアダプターを作成する場合は、アダプターリストへの貢献のためにPRをよろしくお願いいたします！

## 📂 インストール
OpenTetsu をプロジェクトにインストールするには:

1.  リリースに移動して、最新の OpenTetsu API ライブラリ DLL ファイルをダウンロードします。
2.  同じセクションで、シミュレーターに適したアダプターを見つけてダウンロードします。
3.  プロジェクトに OpenTetsu を参照します。
    *   参照の追加についてのヘルプが必要ですか？ [Visual Studio のドキュメント](https://learn.microsoft.com/ja-jp/visualstudio/ide/how-to-add-or-remove-references-by-using-the-reference-manager?view=vs-2022) または [JetBrains Rider のドキュメント](https://www.jetbrains.com/help/rider/Extending_Your_Solution.html#project_assembly_references) を参照してください。
4.  NuGet から必要な依存関係である `Newtonsoft.Json` をインストールします。
5.  以上です！

## ⌨️ 使用法
### アダプターを使用する場合
この例は、TRAIN CREW での使用を想定して `OpenTetsu.Adapters.TrainCrewAdapter` を使用して記述されています。

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

### アダプターを使用しない場合 - 独自のアダプターを使用

アダプターなしで OpenTetsu API を使用する場合は、OpenTetsuData クラスを手動で埋める必要があります。

以下にクラスのリストを示し、既存のアダプターのコードを参照して独自のアダプターを作成できます（このリポジトリの `OpenTetsu.Adapters` に既存のアダプターがあります）。

<details>
<summary>OpenTetsuDataクラスのリスト</summary>

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

## 例

<details>
<summary>サンプルデータを表示</summary>

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
</details>

## 💾 タヌ電OSS
このリポジトリのソースコードはオープンソースです。[Mozilla Public License 2.0ライセンス](https://github.com/haruyukitanuki/OpenTetsu/blob/main/LICENSE.md)に従って、無償で内容を変更、共有、配布することができます。

## 💝 応援をよろしくお願いいたします。
[狸河電鉄公式Discordサーバー](https://go.tanu.ch/tanuden-discord)・
[ツイッター](https://go.tanu.ch/twitter)・[YouTube](https://go.tanu.ch/tanutube)

**狸河電鉄作品｜Copyright &copy; 2024 狸治 明志（Haruyuki Tanukiji）**
