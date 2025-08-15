import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProducedCoilDetailComponent } from './produced-coil-form.component';

describe('ProducedCoilDetailComponent', () => {
  let component: ProducedCoilDetailComponent;
  let fixture: ComponentFixture<ProducedCoilDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProducedCoilDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProducedCoilDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
