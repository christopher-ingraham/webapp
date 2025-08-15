import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RollsSmallComponent } from './rolls-small.component';

describe('RollsSmallComponent', () => {
  let component: RollsSmallComponent;
  let fixture: ComponentFixture<RollsSmallComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RollsSmallComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RollsSmallComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
