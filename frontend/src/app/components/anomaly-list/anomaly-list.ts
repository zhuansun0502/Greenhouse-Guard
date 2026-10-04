import { DatePipe, DecimalPipe } from '@angular/common';
import { Component, input } from '@angular/core';
import { Anomaly } from '../../models';

@Component({
  imports: [DecimalPipe, DatePipe],
  selector: 'app-anomaly-list',
  styleUrl: './anomaly-list.scss',
  templateUrl: './anomaly-list.html',
})

export class AnomalyList {
  readonly anomalies = input.required<Anomaly[]>()
}
