import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DowncoilerComponent } from './downcoiler.component';

describe('DowncoilerComponent', () => {
  let component: DowncoilerComponent;
  let fixture: ComponentFixture<DowncoilerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DowncoilerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DowncoilerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
