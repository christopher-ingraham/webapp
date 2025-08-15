import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionRootComponent } from './production-root.component';

describe('ProductionRootComponent', () => {
  let component: ProductionRootComponent;
  let fixture: ComponentFixture<ProductionRootComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductionRootComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductionRootComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
