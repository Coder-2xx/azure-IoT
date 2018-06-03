import { TestBed, inject } from '@angular/core/testing';

import { DevicesConstantsService } from './devices.constants.service';

describe('Devices.ConstantsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DevicesConstantsService]
    });
  });

  it('should be created', inject([DevicesConstantsService], (service: DevicesConstantsService) => {
    expect(service).toBeTruthy();
  }));
});
