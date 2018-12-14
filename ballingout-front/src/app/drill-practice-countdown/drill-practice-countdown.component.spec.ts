import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DrillPracticeCountdownComponent } from './drill-practice-countdown.component';

describe('DrillPracticeCountdownComponent', () => {
  let component: DrillPracticeCountdownComponent;
  let fixture: ComponentFixture<DrillPracticeCountdownComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DrillPracticeCountdownComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DrillPracticeCountdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
