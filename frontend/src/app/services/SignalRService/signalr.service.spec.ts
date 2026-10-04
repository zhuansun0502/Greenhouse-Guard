import { TestBed } from '@angular/core/testing';
import { SignalRService } from './signalr.service';
import { SensorReading } from '../../models';
import { SensorDataService } from '../SensorDataService/sensor-data.service';

describe('SignalRService', () => {
  let dataService: SensorDataService;
  let signalRService: SignalRService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    dataService = TestBed.inject(SensorDataService);
    signalRService = TestBed.inject(SignalRService);
  });

  it('should be created', () => {
    expect(signalRService).toBeTruthy();
    expect(dataService).toBeTruthy();
  });

  it('should update sensor reading on SignalR message', () => {
    const reading: SensorReading = {
      id: crypto.randomUUID(),
      sequenceNumber: 0,
      timestamp: '2026-10-04T10:00:00Z',
      temperature: 24.5,
      humidity: 65,
      co2Ppm: 700,
    };

    signalRService.sensorReading$.next(reading);
    let latestReading: SensorReading | null = null;
    dataService.getCurrentReading().subscribe(r => latestReading = r);
    expect(latestReading).toEqual(reading);
  });
});
