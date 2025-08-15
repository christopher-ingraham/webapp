import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeReportFiltersComponent } from './practice-report-filters.component';

describe('PracticeReportFiltersComponent', () => {
  let component: PracticeReportFiltersComponent;
  let fixture: ComponentFixture<PracticeReportFiltersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeReportFiltersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeReportFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
