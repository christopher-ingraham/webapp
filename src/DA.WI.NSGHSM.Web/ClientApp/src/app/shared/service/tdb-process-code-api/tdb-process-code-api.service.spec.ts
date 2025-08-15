import { TestBed } from '@angular/core/testing';

import { TdbProcessCodeApiService } from './tdb-process-code-api.service';

describe('TdbProcessCodeApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TdbProcessCodeApiService = TestBed.get(TdbProcessCodeApiService);
    expect(service).toBeTruthy();
  });
});
