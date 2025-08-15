import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeOrientedReportFiltersComponent } from './time-oriented-report-filters.component';

describe('TimeOrientedReportFiltersComponent', () => {
  let component: TimeOrientedReportFiltersComponent;
  let fixture: ComponentFixture<TimeOrientedReportFiltersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimeOrientedReportFiltersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeOrientedReportFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
