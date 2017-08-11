import { TestBed, inject } from '@angular/core/testing';

import { SystemMenuService } from './system-menu.service';

describe('SystemMenuService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SystemMenuService]
    });
  });

  it('should be created', inject([SystemMenuService], (service: SystemMenuService) => {
    expect(service).toBeTruthy();
  }));
});
