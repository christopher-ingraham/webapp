import { TestBed } from '@angular/core/testing';

import { RepHmSetupApiService } from './rep-hm-setup-api.service';

describe('RepHmSetupApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RepHmSetupApiService = TestBed.get(RepHmSetupApiService);
    expect(service).toBeTruthy();
  });
});
