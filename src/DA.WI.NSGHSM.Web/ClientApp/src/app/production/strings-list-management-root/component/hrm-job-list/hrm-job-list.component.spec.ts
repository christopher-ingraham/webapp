import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmJobListComponent } from './hrm-job-list.component';

describe('HrmJobListComponent', () => {
  let component: HrmJobListComponent;
  let fixture: ComponentFixture<HrmJobListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HrmJobListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmJobListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
