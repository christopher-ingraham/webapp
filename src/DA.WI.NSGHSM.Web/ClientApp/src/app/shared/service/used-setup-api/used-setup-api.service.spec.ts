import { TestBed } from '@angular/core/testing';

import { UsedSetupApiService } from './used-setup-api.service';

describe('UsedSetupApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UsedSetupApiService = TestBed.get(UsedSetupApiService);
    expect(service).toBeTruthy();
  });
});
