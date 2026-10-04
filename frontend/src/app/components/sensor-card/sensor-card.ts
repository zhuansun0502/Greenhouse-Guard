import { DatePipe } from '@angular/common';
import { Component, input } from '@angular/core';

@Component({
  imports: [DatePipe],
  selector: 'app-sensor-card',
  styleUrl: './sensor-card.scss',
  templateUrl: './sensor-card.html',
})
export class SensorCard {
  readonly title = input.required();
  readonly timestamp = input.required<string | undefined>();
  readonly value = input.required();
}
