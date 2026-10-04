import { Component, inject, OnInit, signal } from '@angular/core';
import { Header } from './components/header/header';
import { SensorDataService } from './services/SensorDataService/sensor-data.service';
import { SignalRService } from './services/SignalRService/signalr.service';
import { SensorCard } from './components/sensor-card/sensor-card';
import { AnomalyList } from './components/anomaly-list/anomaly-list';
import { toSignal } from '@angular/core/rxjs-interop';
import { DatePipe } from '@angular/common';

@Component({
  imports: [Header, SensorCard, AnomalyList, DatePipe],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App implements OnInit {
  private readonly sensorDataService = inject(SensorDataService);

  readonly latestReading = toSignal(this.sensorDataService.getCurrentReading());
  readonly anomalyList = toSignal(this.sensorDataService.getAnomalies());

  ngOnInit() {
    
  }
}
