import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MainSignalsRootComponent } from './main-signals-root.component';

describe('MainSignalsRootComponent', () => {
  let component: MainSignalsRootComponent;
  let fixture: ComponentFixture<MainSignalsRootComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MainSignalsRootComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MainSignalsRootComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
