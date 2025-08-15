import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InputSlabsManagementRootComponent } from './input-slabs-management-root.component';

describe('InputSlabsManagementRootComponent', () => {
  let component: InputSlabsManagementRootComponent;
  let fixture: ComponentFixture<InputSlabsManagementRootComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InputSlabsManagementRootComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InputSlabsManagementRootComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
