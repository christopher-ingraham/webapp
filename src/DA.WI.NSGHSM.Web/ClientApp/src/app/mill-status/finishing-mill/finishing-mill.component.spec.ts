import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FinishingMillComponent } from './finishing-mill.component';

describe('FinishingMillComponent', () => {
  let component: FinishingMillComponent;
  let fixture: ComponentFixture<FinishingMillComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FinishingMillComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FinishingMillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
