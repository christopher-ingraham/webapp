import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RepHmMapsViewListComponent } from './rep-hm-maps-view-list.component';

describe('RepHmMapsViewListComponent', () => {
  let component: RepHmMapsViewListComponent;
  let fixture: ComponentFixture<RepHmMapsViewListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RepHmMapsViewListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RepHmMapsViewListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
