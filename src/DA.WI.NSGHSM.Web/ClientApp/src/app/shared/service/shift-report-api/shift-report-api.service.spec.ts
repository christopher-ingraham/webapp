import { TestBed } from '@angular/core/testing';

import { ShiftReportApiService } from './shift-report-api.service';

describe('ShiftReportApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ShiftReportApiService = TestBed.get(ShiftReportApiService);
    expect(service).toBeTruthy();
  });
});
