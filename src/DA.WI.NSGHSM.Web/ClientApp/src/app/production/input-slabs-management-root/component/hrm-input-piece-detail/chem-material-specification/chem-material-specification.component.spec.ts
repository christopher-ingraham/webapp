import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemMaterialSpecificationComponent } from './chem-material-specification.component';

describe('ChemMaterialSpecificationComponent', () => {
  let component: ChemMaterialSpecificationComponent;
  let fixture: ComponentFixture<ChemMaterialSpecificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChemMaterialSpecificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemMaterialSpecificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
