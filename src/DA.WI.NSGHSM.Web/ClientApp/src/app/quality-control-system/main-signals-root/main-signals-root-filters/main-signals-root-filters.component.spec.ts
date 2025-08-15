import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MainSignalsRootFiltersComponent } from './main-signals-root-filters.component';

describe('MainSignalsRootFiltersComponent', () => {
  let component: MainSignalsRootFiltersComponent;
  let fixture: ComponentFixture<MainSignalsRootFiltersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MainSignalsRootFiltersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MainSignalsRootFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
