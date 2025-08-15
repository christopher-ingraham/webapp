import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EdgerComponent } from './edger.component';

describe('EdgerComponent', () => {
  let component: EdgerComponent;
  let fixture: ComponentFixture<EdgerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EdgerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EdgerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
