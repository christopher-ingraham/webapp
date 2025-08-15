import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmInputPieceListComponent } from './hrm-input-piece-list.component';

describe('HrmInputPieceListComponent', () => {
  let component: HrmInputPieceListComponent;
  let fixture: ComponentFixture<HrmInputPieceListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HrmInputPieceListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmInputPieceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
