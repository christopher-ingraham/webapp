import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeOrientedReportComponent } from './time-oriented-report.component';

describe('TimeOrientedReportComponent', () => {
  let component: TimeOrientedReportComponent;
  let fixture: ComponentFixture<TimeOrientedReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimeOrientedReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeOrientedReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
