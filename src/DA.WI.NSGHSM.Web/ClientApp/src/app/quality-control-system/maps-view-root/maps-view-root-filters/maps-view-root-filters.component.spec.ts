import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapsViewRootFiltersComponent } from './maps-view-root-filters.component';

describe('MapsViewRootFiltersComponent', () => {
  let component: MapsViewRootFiltersComponent;
  let fixture: ComponentFixture<MapsViewRootFiltersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapsViewRootFiltersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapsViewRootFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
