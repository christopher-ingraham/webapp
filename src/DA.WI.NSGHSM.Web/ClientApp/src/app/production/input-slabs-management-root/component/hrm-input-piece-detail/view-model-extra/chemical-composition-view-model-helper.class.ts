import { BehaviorSubject } from 'rxjs';

import { ChemicalCompositionIndex } from './chemical-composition-index.enum';

export class ChemicalCompositionViewModelHelper {
    // Is Chemical composition tab visible?
    public readonly visible: BehaviorSubject<boolean>;

    public get isVisible(): boolean {
        return this.visible.getValue();
    }
    public set isVisible(value: boolean) {
        this.visible.next(value);
    }

    // Which subsection is visible?
    public readonly index: BehaviorSubject<ChemicalCompositionIndex>;
    public get chemSubTab(): ChemicalCompositionIndex {
        return this.index.getValue();
    }
    public set chemSubTab(value: ChemicalCompositionIndex) {
        this.index.next(value);
    }

    public readonly indexLaboratoryAnalysis = ChemicalCompositionIndex.LaboratoryAnalysis;
    public get isLaboratoryAnalysisVisible(): boolean {
        return (this.index.getValue() === ChemicalCompositionIndex.LaboratoryAnalysis);
    }
    public set isLaboratoryAnalysisVisible(value: boolean) {
        if (value) {
            this.index.next(ChemicalCompositionIndex.LaboratoryAnalysis);
        }
    }

    public readonly indexSteelGrade = ChemicalCompositionIndex.SteelGrade;
    public get isSteelGradeVisible(): boolean {
        return (this.index.getValue() === ChemicalCompositionIndex.SteelGrade);
    }
    public set isSteelGradeVisible(value: boolean) {
        if (value) {
            this.index.next(ChemicalCompositionIndex.SteelGrade);
        }
    }

    public readonly indexMaterialSpecification = ChemicalCompositionIndex.MaterialSpecification;
    public get isMaterialSpecificationVisible(): boolean {
        return (this.index.getValue() === ChemicalCompositionIndex.MaterialSpecification);
    }
    public set isMaterialSpecificationVisible(value: boolean) {
        if (value) {
            this.index.next(ChemicalCompositionIndex.MaterialSpecification);
        }
    }

    // Is default steel grade used?
    public readonly defaultSteelGradeUsed: BehaviorSubject<boolean>;
    public get isDefaultSteelGradeUsed(): boolean {
        return this.defaultSteelGradeUsed.getValue();
    }
    public set isDefaultSteelGradeUsed(value: boolean) {
        this.defaultSteelGradeUsed.next(value);
    }

    constructor() {
        this.visible = new BehaviorSubject<boolean>(false);
        this.index = new BehaviorSubject<ChemicalCompositionIndex>(ChemicalCompositionIndex.LaboratoryAnalysis);
        this.defaultSteelGradeUsed = new BehaviorSubject<boolean>(false);
    }
}
