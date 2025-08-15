import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RepHmPieceTrendDetailComponent } from './rep-hm-piece-trend-detail.component';

describe('RepHmPieceTrendDetailComponent', () => {
  let component: RepHmPieceTrendDetailComponent;
  let fixture: ComponentFixture<RepHmPieceTrendDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RepHmPieceTrendDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RepHmPieceTrendDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
