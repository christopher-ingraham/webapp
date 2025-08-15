import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemLaboratoryAnalysisComponent } from './chem-laboratory-analysis.component';

describe('ChemLaboratoryAnalysisComponent', () => {
  let component: ChemLaboratoryAnalysisComponent;
  let fixture: ComponentFixture<ChemLaboratoryAnalysisComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChemLaboratoryAnalysisComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemLaboratoryAnalysisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
