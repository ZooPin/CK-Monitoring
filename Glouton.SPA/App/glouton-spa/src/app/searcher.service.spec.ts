import { TestBed, inject } from '@angular/core/testing';

import { SearcherService } from './searcher.service';

describe('SearcherService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SearcherService]
    });
  });

  it('should be created', inject([SearcherService], (service: SearcherService) => {
    expect(service).toBeTruthy();
  }));
});
