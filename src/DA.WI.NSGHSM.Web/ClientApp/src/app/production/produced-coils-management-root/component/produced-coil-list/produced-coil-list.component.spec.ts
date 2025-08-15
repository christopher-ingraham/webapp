import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProducedCoilListComponent } from './produced-coil-list.component';

describe('ProducedCoilListComponent', () => {
  let component: ProducedCoilListComponent;
  let fixture: ComponentFixture<ProducedCoilListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProducedCoilListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProducedCoilListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
