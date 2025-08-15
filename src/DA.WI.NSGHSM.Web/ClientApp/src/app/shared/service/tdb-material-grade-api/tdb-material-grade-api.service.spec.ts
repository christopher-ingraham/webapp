import { TestBed } from '@angular/core/testing';

import { TdbMaterialGradeApiService } from './tdb-material-grade-api.service';

describe('TdbMaterialGradeApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TdbMaterialGradeApiService = TestBed.get(TdbMaterialGradeApiService);
    expect(service).toBeTruthy();
  });
});
