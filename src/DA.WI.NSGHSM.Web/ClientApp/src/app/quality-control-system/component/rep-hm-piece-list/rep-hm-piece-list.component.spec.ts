import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RepHmPieceListComponent } from './rep-hm-piece-list.component';

describe('RepHmPieceListComponent', () => {
  let component: RepHmPieceListComponent;
  let fixture: ComponentFixture<RepHmPieceListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RepHmPieceListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RepHmPieceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
