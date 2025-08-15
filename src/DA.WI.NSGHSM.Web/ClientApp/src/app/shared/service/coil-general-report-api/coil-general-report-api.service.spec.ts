import { TestBed } from '@angular/core/testing';

import { CoilGeneralReportApiService } from './coil-general-report-api.service';

describe('CoilGeneralReportApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CoilGeneralReportApiService = TestBed.get(CoilGeneralReportApiService);
    expect(service).toBeTruthy();
  });
});
