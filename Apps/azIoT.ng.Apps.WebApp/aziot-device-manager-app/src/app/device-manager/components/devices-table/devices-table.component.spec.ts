
import { fakeAsync, ComponentFixture, TestBed } from '@angular/core/testing';

import { DevicesTableComponent } from './devices-table.component';

describe('DevicesTableComponent', () => {
  let component: DevicesTableComponent;
  let fixture: ComponentFixture<DevicesTableComponent>;

  beforeEach(fakeAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ DevicesTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DevicesTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
