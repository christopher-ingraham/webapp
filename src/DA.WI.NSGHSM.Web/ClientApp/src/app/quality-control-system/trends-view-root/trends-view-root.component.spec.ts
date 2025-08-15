import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrendsViewRootComponent } from './trends-view-root.component';

describe('TrendsViewRootComponent', () => {
  let component: TrendsViewRootComponent;
  let fixture: ComponentFixture<TrendsViewRootComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrendsViewRootComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrendsViewRootComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
