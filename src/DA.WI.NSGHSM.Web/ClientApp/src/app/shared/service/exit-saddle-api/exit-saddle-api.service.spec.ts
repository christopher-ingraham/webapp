import { TestBed } from '@angular/core/testing';

import { ExitSaddleApiService } from './exit-saddle-api.service';

describe('ExitSaddleApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ExitSaddleApiService = TestBed.get(ExitSaddleApiService);
    expect(service).toBeTruthy();
  });
});
