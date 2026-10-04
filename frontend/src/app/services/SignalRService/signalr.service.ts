import { Injectable } from '@angular/core';
import { Anomaly, ConnectionStatus, SensorReading } from '../../models';
import { BehaviorSubject, Subject } from 'rxjs';
import * as signalR from '@microsoft/signalr';

const CONNECTION_RETRY_INTERVAL = 3000;

@Injectable({ providedIn: 'root' })
export class SignalRService {
    public sensorReading$ = new Subject<SensorReading>();
    public anomaly$ = new Subject<Anomaly>();
    public connectionStatus$ = new BehaviorSubject<ConnectionStatus>('disconnected');

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
        this.hub.onreconnected(() => this.connectionStatus$.next('connected'));
        this.hub.onclose(() => this.connectionStatus$.next('disconnected'));
    }

    async connect() {
        this.connectionStatus$.next('connecting');

        try {
            await this.hub.start();
            this.connectionStatus$.next('connected');
        } catch (error) {
            this.connectionStatus$.next('disconnected');
            console.error('Error connecting to SignalR:', (error as Error).message, '- retrying in 3 seconds');
            setTimeout(() => this.connect(), CONNECTION_RETRY_INTERVAL);
        }
    }

    async disconnect() {
        try {
            await this.hub.stop();
            clearTimeout(this.retryTimer);
            this.connectionStatus$.next('disconnected');
        } catch (error) {
            console.error('Error disconnecting from SignalR:', (error as Error).message);
        }
    }

    getConnectionStatus() {
        return this.connectionStatus$;
    }
}
