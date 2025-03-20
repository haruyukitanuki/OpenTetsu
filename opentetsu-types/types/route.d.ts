export type StopType = "PassengerStop" | "OperationStop" | "Passing";

export interface StationTimings {
  arrival: Date;
  departure: Date;
}

export interface Station {
  distance: number;
  index: number;
  name: string;
  positionName: string;
  stopType: StopType;
  timings: StationTimings;
}

export type Direction = "Inbound" | "Outbound" | "Both" | null;

export interface Diagram {
  direction: Direction;
  boundFor: string;
  remainingDistance: number;
  serviceType: string;
  stations: Station[];
}

export interface NextStation extends Station {
  distanceFromTrain: number;
}
