import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportListRootComponent } from './report-list-root.component';

describe('ReportListRootComponent', () => {
  let component: ReportListRootComponent;
  let fixture: ComponentFixture<ReportListRootComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportListRootComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportListRootComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
