import { TestBed } from '@angular/core/testing';

import { RmlCrewApiService } from './rml-crew-api.service';

describe('RmlCrewApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RmlCrewApiService = TestBed.get(RmlCrewApiService);
    expect(service).toBeTruthy();
  });
});
