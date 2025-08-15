import { TestBed } from '@angular/core/testing';

import { HrmAnalysisDataApiService } from './hrm-analysis-data-api.service';

describe('HrmAnalysisDataApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: HrmAnalysisDataApiService = TestBed.get(HrmAnalysisDataApiService);
    expect(service).toBeTruthy();
  });
});
