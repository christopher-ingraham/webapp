import { TestBed } from '@angular/core/testing';

import { AppStatusStoreService } from './app-status-store.service';

describe('AppStatusStoreService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AppStatusStoreService = TestBed.get(AppStatusStoreService);
    expect(service).toBeTruthy();
  });
});
