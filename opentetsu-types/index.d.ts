export interface OpenTetsuData {
  runNumber?: string; // 運行番号
  diagramNumber?: string; // 列車番号
  currentTime: string;
  diagram: Diagram;
  nextStation: NextStation;
  train: TrainState;
  signalStates: SignalState[];
  atsState: AtsState;
  controllerState: ControllerState;
}

export interface AtsState {
  stopPattern: string;
  speed: number;
  state: string;
}

export interface ControllerState {
  notch: number;
  bNotch: number;
  pNotch: number;
  reverser: number;
}

export type StopType = "PassengerStop" | "OperationStop" | "Passing";

export interface StationTimings {
  arrival: string;
  departure: string;
}

export type DoorDirection = "LeftSide" | "RightSide";

export interface Station {
  distance: number;
  index: number;
  name: string;
  positionName: string;
  doorDirection: DoorDirection;
  stopType: StopType;
  timings: StationTimings;
}

export type Direction = "Inbound" | "Outbound" | "Both" | null;

interface Diagram {
  direction: Direction;
  boundFor: string;
  remainingDistance: number;
  serviceType: string;
  stations: Station[];
}

export interface NextStation extends Station {
  distanceFromTrain: number;
}

export type SignalType = "Standard" | "Switch" | "Home" | "Departure";

export interface Transponder {
  type: string;
  speedLimit: number;
  distance: number;
}

export interface SignalState {
  name: string;
  type: SignalType;
  phase: string;
  distance: number;
  transponders: Transponder[];
}

export type PantographType = "SingleArm" | "ScissorArm";

export interface CarProperties {
  driverCab: boolean;
  conductorCab: boolean;
  motor: boolean;
  cabDirection: Direction;
  pantograph: boolean;
  pantographDirection: Direction;
  pantographType: PantographType;
}

export interface CarStateBase {
  carNo: number;
  model: string;
  properties: CarProperties;
}

export interface CarState extends CarStateBase {
  amperage: number;
  bcPressure: number;
  isDoorClosed: boolean;
}

export interface LampsAts {
  brakeApplication: boolean;
  inOperation: boolean;
  isolated: boolean;
}

export interface Lamps {
  ats: LampsAts;
  eBrake: boolean;
  ebTimer: boolean;
  overload: boolean;
  pilot: boolean;
  regenBrake: boolean;
}

export type SpeedLimitType = "Signal" | "SpeedLimit";

export interface NextSpeedLimit {
  distance: number;
  limit: number;
  type: SpeedLimitType;
}

export interface TrainStateProperties {
  model: string;
  holdBrake: boolean;
  pantographType: PantographType;
  pantographDirections: Map<number, Direction[] | null>;
}

export interface TrainState {
  cars: CarState[];
  consist: number;
  lamps: Lamps;
  mrPressure: number;
  nextSpeedLimit: NextSpeedLimit;
  speed: number;
  speedLimit: number;
  speedLimitType: SpeedLimitType;
  gradient: number;
  distance: number;
  properties: TrainStateProperties;
}
