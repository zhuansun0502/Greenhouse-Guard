export interface SensorReading {
    id: string;
    sequenceNumber: number;
    timestamp: string;
    temperature: number;
    humidity: number;
    co2Ppm: number;
  }
  
  export interface Anomaly {
    id: string;
    detectedAt: string;
    sensorType: string;
    value: number;
    zScore: number;
    reason: string;
  }
  
  export type ConnectionStatus = 'OFFLINE' | 'connecting' | 'connected' | 'reconnecting' | 'disconnected' | 'LIVE';

  export const API_ENDPOINT_BASE = 'http://localhost:5086';