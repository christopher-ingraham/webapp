import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapsViewRootComponent } from './maps-view-root.component';

describe('MapsViewRootComponent', () => {
  let component: MapsViewRootComponent;
  let fixture: ComponentFixture<MapsViewRootComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapsViewRootComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapsViewRootComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
