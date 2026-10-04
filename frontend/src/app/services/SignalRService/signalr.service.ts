import { Injectable, OnDestroy } from '@angular/core';
import { Anomaly, ConnectionStatus, SensorReading } from '../../models';
import { BehaviorSubject, Subject } from 'rxjs';
import * as signalR from '@microsoft/signalr';

const CONNECTION_RETRY_INTERVAL = 3000;

@Injectable({ providedIn: 'root' })
export class SignalRService implements OnDestroy {
    public sensorReading$ = new Subject<SensorReading>();
    public anomaly$ = new Subject<Anomaly>();
    public connectionStatus$ = new BehaviorSubject<ConnectionStatus>('OFFLINE');

    private readonly hub = new signalR.HubConnectionBuilder()
        .withUrl('http://localhost:5086/live')
        .withAutomaticReconnect()
        .build();

    private readonly retryTimer = setTimeout(() => this.connect(), CONNECTION_RETRY_INTERVAL);

    constructor() {
        this.hub.on('NewReading', (reading: SensorReading) => this.sensorReading$.next(reading));
        this.hub.on('AnomaliesDetected', (anomalies: Anomaly[]) =>
            anomalies.forEach(anomaly => this.anomaly$.next(anomaly)),
        );

        this.hub.onreconnecting(() => this.connectionStatus$.next('reconnecting'));
        this.hub.onreconnected(() => this.connectionStatus$.next('LIVE'));
        this.hub.onclose(() => this.connectionStatus$.next('OFFLINE'));

        window.addEventListener('offline', () => this.connectionStatus$.next('OFFLINE'));
        window.addEventListener('online', this.handleOnline);
    }

    private readonly handleOnline = () => {
        if (this.hub.state !== signalR.HubConnectionState.Connected)
            this.connect();
        else
            this.connectionStatus$.next('connecting');
    };

    async connect() {
        if (this.hub.state === signalR.HubConnectionState.Connected)
            return;

        this.connectionStatus$.next('connecting');

        try {
            await this.hub.start();
            this.connectionStatus$.next('LIVE');
        } catch (error) {
            this.connectionStatus$.next('OFFLINE');
            console.error('Error connecting to SignalR:', (error as Error).message, '- retrying in 3 seconds');
            setTimeout(() => this.connect(), CONNECTION_RETRY_INTERVAL);
        }
    }

    async disconnect() {
        try {
            await this.hub.stop();
            clearTimeout(this.retryTimer);
            this.connectionStatus$.next('OFFLINE');
        } catch (error) {
            console.error('Error disconnecting from SignalR:', (error as Error).message);
        }
    }

    getConnectionStatus() {
        return this.connectionStatus$;
    }

    ngOnDestroy() {
        this.disconnect();
    }
}
