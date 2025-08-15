import { TestBed } from '@angular/core/testing';

import { PracticeReportApiService } from './practice-report-api.service';

describe('PracticeReportApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PracticeReportApiService = TestBed.get(PracticeReportApiService);
    expect(service).toBeTruthy();
  });
});
