import { AtsState } from "./types/ats";
import { ControllerState } from "./types/controller";
import { Diagram, NextStation } from "./types/route";
import { SignalState } from "./types/signal-state";
import { TrainState } from "./types/train";

export interface OpenTetsuData {
  runNumber?: string; // 運行番号
  diagramNumber?: string; // 列車番号
  currentTime: Date;
  diagram: Diagram;
  nextStation: NextStation;
  train: TrainState;
  signalStates: SignalState[];
  atsState: AtsState;
  controllerState: ControllerState;
}
