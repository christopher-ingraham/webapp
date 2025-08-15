import { TestBed } from '@angular/core/testing';

import { HrmHeatApiService } from './hrm-heat-api.service';

describe('HrmHeatApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: HrmHeatApiService = TestBed.get(HrmHeatApiService);
    expect(service).toBeTruthy();
  });
});
