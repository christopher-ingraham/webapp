import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RollsBigComponent } from './rolls-big.component';

describe('RollsBigComponent', () => {
  let component: RollsBigComponent;
  let fixture: ComponentFixture<RollsBigComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RollsBigComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RollsBigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
