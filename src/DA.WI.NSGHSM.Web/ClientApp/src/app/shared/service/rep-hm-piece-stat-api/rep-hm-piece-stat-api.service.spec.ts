import { TestBed } from '@angular/core/testing';

import { RepHmPieceStatApiService } from './rep-hm-piece-stat-api.service';

describe('RepHmPieceStatApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RepHmPieceStatApiService = TestBed.get(RepHmPieceStatApiService);
    expect(service).toBeTruthy();
  });
});
