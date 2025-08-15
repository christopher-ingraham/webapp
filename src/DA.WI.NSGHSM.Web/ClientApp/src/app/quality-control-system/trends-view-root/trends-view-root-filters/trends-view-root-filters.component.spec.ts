import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrendsViewRootFiltersComponent } from './trends-view-root-filters.component';

describe('TrendsViewRootFiltersComponent', () => {
  let component: TrendsViewRootFiltersComponent;
  let fixture: ComponentFixture<TrendsViewRootFiltersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrendsViewRootFiltersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrendsViewRootFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
