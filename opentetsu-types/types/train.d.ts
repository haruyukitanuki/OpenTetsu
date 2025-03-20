import { Direction } from "./route";

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
