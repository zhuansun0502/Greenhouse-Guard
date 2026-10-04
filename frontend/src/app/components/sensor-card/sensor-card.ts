import { Component, input } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-sensor-card',
  styleUrl: './sensor-card.scss',
  templateUrl: './sensor-card.html',
})
export class SensorCard {
  readonly title = input.required();
  readonly value = input.required();
}
