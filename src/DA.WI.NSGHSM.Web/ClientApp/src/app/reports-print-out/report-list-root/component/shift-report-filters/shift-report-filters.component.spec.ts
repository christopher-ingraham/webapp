import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShiftReportFiltersComponent } from './shift-report-filters.component';

describe('ShiftReportFiltersComponent', () => {
  let component: ShiftReportFiltersComponent;
  let fixture: ComponentFixture<ShiftReportFiltersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShiftReportFiltersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShiftReportFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
