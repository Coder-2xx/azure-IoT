import { TestBed, inject } from '@angular/core/testing';

import { SharedConstantsService } from './shared.constants.service';

describe('Shared.ConstantsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SharedConstantsService]
    });
  });

  it('should be created', inject([SharedConstantsService], (service: SharedConstantsService) => {
    expect(service).toBeTruthy();
  }));
});
