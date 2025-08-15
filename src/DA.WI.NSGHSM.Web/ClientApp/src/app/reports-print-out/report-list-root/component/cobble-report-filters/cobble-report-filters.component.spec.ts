import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CobbleReportFiltersComponent } from './cobble-report-filters.component';

describe('CobbleReportFiltersComponent', () => {
  let component: CobbleReportFiltersComponent;
  let fixture: ComponentFixture<CobbleReportFiltersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CobbleReportFiltersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CobbleReportFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
