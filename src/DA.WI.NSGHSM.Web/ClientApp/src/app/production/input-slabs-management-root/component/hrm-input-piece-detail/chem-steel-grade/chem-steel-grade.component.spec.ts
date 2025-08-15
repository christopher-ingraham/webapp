import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemSteelGradeComponent } from './chem-steel-grade.component';

describe('ChemSteelGradeComponent', () => {
  let component: ChemSteelGradeComponent;
  let fixture: ComponentFixture<ChemSteelGradeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChemSteelGradeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemSteelGradeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
