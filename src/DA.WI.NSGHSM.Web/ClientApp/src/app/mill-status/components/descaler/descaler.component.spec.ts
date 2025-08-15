import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DescalerComponent } from './descaler.component';

describe('DescalerComponent', () => {
  let component: DescalerComponent;
  let fixture: ComponentFixture<DescalerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DescalerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DescalerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
