import { TestBed } from '@angular/core/testing';

import { ProfileInfoService } from './profile-info.service';

describe('ProfileInfoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProfileInfoService = TestBed.get(ProfileInfoService);
    expect(service).toBeTruthy();
  });
});
