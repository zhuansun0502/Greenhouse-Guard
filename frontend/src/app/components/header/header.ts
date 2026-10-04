import { Component, inject, signal } from '@angular/core';
import { SignalRService } from '../../services/SignalRService/signalr.service';
import { SensorDataService } from '../../services/SensorDataService/sensor-data.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { DatePipe } from '@angular/common';

@Component({
  imports: [DatePipe],
  selector: 'app-header',
  styleUrl: './header.scss',
  templateUrl: './header.html',
})

export class Header {
  private readonly signalRService = inject(SignalRService);
  private readonly sensorDataService = inject(SensorDataService);

  readonly latestReading = toSignal(this.sensorDataService.getCurrentReading());
  readonly connectionStatus = toSignal(this.signalRService.getConnectionStatus());
}
