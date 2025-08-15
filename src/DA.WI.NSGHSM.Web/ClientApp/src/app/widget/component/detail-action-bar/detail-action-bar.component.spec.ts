import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailActionBarComponent } from './detail-action-bar.component';

describe('DetailActionBarComponent', () => {
  let component: DetailActionBarComponent;
  let fixture: ComponentFixture<DetailActionBarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetailActionBarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailActionBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
