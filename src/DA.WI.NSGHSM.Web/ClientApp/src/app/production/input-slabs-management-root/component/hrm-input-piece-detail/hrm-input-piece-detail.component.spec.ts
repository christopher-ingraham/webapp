import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmInputPieceDetailComponent } from './hrm-input-piece-form.component';

describe('HrmInputPieceDetailComponent', () => {
  let component: HrmInputPieceDetailComponent;
  let fixture: ComponentFixture<HrmInputPieceDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HrmInputPieceDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmInputPieceDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
