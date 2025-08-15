import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CobbleReportComponent } from './cobble-report.component';

describe('CobbleReportComponent', () => {
  let component: CobbleReportComponent;
  let fixture: ComponentFixture<CobbleReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CobbleReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CobbleReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
