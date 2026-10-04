import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AnomalyList } from './anomaly-list';

describe('AnomalyList', () => {
  let component: AnomalyList;
  let fixture: ComponentFixture<AnomalyList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AnomalyList],
    }).compileComponents();

    fixture = TestBed.createComponent(AnomalyList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
