import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Anomaly, SensorReading } from '../../models';
import { SignalRService } from '../SignalRService/signalr.service';

@Injectable({ providedIn: 'root' })
export class SensorDataService {
    private currentReading$ = new BehaviorSubject<SensorReading | null>(null);
    private anomalies$ = new BehaviorSubject<Anomaly[]>([]);

    private readonly signalRService = inject(SignalRService);

    constructor() {
        this.signalRService.sensorReading$.subscribe((reading) => {
            this.currentReading$.next(reading);
        });
        this.signalRService.anomaly$.subscribe((anomaly) => {
            this.anomalies$.next([...this.anomalies$.value, anomaly].slice(-10));
        });
    }

    getCurrentReading(): Observable<SensorReading | null> {
        return this.currentReading$;
    }

    getAnomalies(): Observable<Anomaly[]> {
        return this.anomalies$;
    }

    ngOnDestroy() {
        this.signalRService.sensorReading$.unsubscribe();
        this.signalRService.anomaly$.unsubscribe();
    }
}
