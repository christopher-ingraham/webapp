import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CoilGeneralReportComponent } from './coil-general-report.component';

describe('CoilGeneralReportComponent', () => {
  let component: CoilGeneralReportComponent;
  let fixture: ComponentFixture<CoilGeneralReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CoilGeneralReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CoilGeneralReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
