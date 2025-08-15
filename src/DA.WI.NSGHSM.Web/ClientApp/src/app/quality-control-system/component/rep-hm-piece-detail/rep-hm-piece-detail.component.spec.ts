import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RepHmPieceDetailComponent } from './rep-hm-piece-form.component';

describe('RepHmPieceDetailComponent', () => {
  let component: RepHmPieceDetailComponent;
  let fixture: ComponentFixture<RepHmPieceDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RepHmPieceDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RepHmPieceDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
