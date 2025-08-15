import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UsedSetupListComponent } from './used-setup-list.component';

describe('UsedSetupListComponent', () => {
  let component: UsedSetupListComponent;
  let fixture: ComponentFixture<UsedSetupListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UsedSetupListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsedSetupListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
