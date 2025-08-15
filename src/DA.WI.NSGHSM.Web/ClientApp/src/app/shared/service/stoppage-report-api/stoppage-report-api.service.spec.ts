import { TestBed } from '@angular/core/testing';

import { StoppageReportApiService } from './stoppage-report-api.service';

describe('StoppageReportApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: StoppageReportApiService = TestBed.get(StoppageReportApiService);
    expect(service).toBeTruthy();
  });
});
