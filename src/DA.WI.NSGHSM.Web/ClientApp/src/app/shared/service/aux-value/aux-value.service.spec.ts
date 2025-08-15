import { TestBed } from '@angular/core/testing';

import { AuxValueService } from './aux-value.service';

describe('AuxValueService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AuxValueService = TestBed.get(AuxValueService);
    expect(service).toBeTruthy();
  });
});
