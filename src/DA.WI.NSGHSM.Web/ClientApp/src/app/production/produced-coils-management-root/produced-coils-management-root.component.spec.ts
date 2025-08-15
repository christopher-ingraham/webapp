import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProducedCoilsManagementRootComponent } from './produced-coils-management-root.component';

describe('ProducedCoilsManagementRootComponent', () => {
  let component: ProducedCoilsManagementRootComponent;
  let fixture: ComponentFixture<ProducedCoilsManagementRootComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProducedCoilsManagementRootComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProducedCoilsManagementRootComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
