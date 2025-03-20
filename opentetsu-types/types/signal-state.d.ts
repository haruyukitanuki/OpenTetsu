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
  transponder: Transponder[];
}
