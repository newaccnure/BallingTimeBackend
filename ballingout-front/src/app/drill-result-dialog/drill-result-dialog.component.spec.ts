import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DrillResultDialogComponent } from './drill-result-dialog.component';

describe('DrillResultDialogComponent', () => {
  let component: DrillResultDialogComponent;
  let fixture: ComponentFixture<DrillResultDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DrillResultDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DrillResultDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
