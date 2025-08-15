import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RepHmPieceTrendListComponent } from './rep-hm-piece-trend-list.component';

describe('RepHmPieceTrendListComponent', () => {
  let component: RepHmPieceTrendListComponent;
  let fixture: ComponentFixture<RepHmPieceTrendListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RepHmPieceTrendListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RepHmPieceTrendListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
