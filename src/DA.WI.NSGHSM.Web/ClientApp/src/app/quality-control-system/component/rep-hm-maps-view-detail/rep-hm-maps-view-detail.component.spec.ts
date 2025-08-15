import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RepHmMapsViewDetailComponent } from './rep-hm-maps-view-detail.component';

describe('RepHmMapsViewDetailComponent', () => {
  let component: RepHmMapsViewDetailComponent;
  let fixture: ComponentFixture<RepHmMapsViewDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RepHmMapsViewDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RepHmMapsViewDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
