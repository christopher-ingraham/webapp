import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmJobDetailComponent } from './hrm-job-form.component';

describe('HrmJobDetailComponent', () => {
  let component: HrmJobDetailComponent;
  let fixture: ComponentFixture<HrmJobDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HrmJobDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmJobDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
