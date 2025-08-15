import { TestBed } from '@angular/core/testing';

import { ProducedCoilApiService } from './produced-coil-api.service';

describe('ProducedCoilApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProducedCoilApiService = TestBed.get(ProducedCoilApiService);
    expect(service).toBeTruthy();
  });
});
