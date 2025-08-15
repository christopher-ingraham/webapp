import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CoolingDowncoilersComponent } from './cooling-downcoilers.component';

describe('CoolingDowncoilersComponent', () => {
  let component: CoolingDowncoilersComponent;
  let fixture: ComponentFixture<CoolingDowncoilersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CoolingDowncoilersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CoolingDowncoilersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
