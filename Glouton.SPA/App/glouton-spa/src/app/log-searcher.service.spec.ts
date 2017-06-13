import { TestBed, inject } from '@angular/core/testing';

import { LogSearcherService } from './log-searcher.service';

describe('LogSearcherService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LogSearcherService]
    });
  });

  it('should be created', inject([LogSearcherService], (service: LogSearcherService) => {
    expect(service).toBeTruthy();
  }));
});
