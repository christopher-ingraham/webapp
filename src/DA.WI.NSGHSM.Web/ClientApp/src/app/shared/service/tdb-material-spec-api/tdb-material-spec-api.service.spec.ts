import { TestBed } from '@angular/core/testing';

import { TdbMaterialSpecApiService } from './tdb-material-spec-api.service';

describe('TdbMaterialSpecApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TdbMaterialSpecApiService = TestBed.get(TdbMaterialSpecApiService);
    expect(service).toBeTruthy();
  });
});
