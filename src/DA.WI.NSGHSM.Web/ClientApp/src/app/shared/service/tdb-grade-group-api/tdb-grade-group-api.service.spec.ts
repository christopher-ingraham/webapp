import { TestBed } from '@angular/core/testing';

import { TdbGradeGroupApiService } from './tdb-grade-group-api.service';

describe('TdbGradeGroupApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TdbGradeGroupApiService = TestBed.get(TdbGradeGroupApiService);
    expect(service).toBeTruthy();
  });
});
