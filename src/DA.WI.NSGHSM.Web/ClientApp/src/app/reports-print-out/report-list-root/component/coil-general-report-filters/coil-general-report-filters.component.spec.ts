import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CoilGeneralReportFiltersComponent } from './coil-general-report-filters.component';

describe('CoilGeneralReportFiltersComponent', () => {
  let component: CoilGeneralReportFiltersComponent;
  let fixture: ComponentFixture<CoilGeneralReportFiltersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CoilGeneralReportFiltersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CoilGeneralReportFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
