import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExitSaddleListComponent } from './exit-saddle-list.component';

describe('ExitSaddleListComponent', () => {
  let component: ExitSaddleListComponent;
  let fixture: ComponentFixture<ExitSaddleListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExitSaddleListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExitSaddleListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
