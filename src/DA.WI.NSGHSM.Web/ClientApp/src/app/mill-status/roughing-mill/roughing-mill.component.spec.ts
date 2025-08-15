import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoughingMillComponent } from './roughing-mill.component';

describe('RoughingMillComponent', () => {
  let component: RoughingMillComponent;
  let fixture: ComponentFixture<RoughingMillComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoughingMillComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoughingMillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
