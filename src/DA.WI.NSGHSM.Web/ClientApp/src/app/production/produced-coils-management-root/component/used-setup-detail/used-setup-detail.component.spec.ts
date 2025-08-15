import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UsedSetupDetailComponent } from './used-setup-form.component';

describe('UsedSetupDetailComponent', () => {
  let component: UsedSetupDetailComponent;
  let fixture: ComponentFixture<UsedSetupDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UsedSetupDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsedSetupDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
