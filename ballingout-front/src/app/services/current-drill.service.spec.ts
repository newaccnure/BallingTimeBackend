import { TestBed } from '@angular/core/testing';

import { CurrentDrillService } from './current-drill.service';

describe('CurrentDrillService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CurrentDrillService = TestBed.get(CurrentDrillService);
    expect(service).toBeTruthy();
  });
});
