import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StandComponent } from './stand.component';

describe('MachineComponent', () => {
  let component: StandComponent;
  let fixture: ComponentFixture<StandComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StandComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StandComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
