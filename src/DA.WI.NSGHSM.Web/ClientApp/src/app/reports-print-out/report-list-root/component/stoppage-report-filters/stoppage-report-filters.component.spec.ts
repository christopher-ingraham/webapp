import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StoppageReportFiltersComponent } from './stoppage-report-filters.component';

describe('StoppageReportFiltersComponent', () => {
  let component: StoppageReportFiltersComponent;
  let fixture: ComponentFixture<StoppageReportFiltersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StoppageReportFiltersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StoppageReportFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
