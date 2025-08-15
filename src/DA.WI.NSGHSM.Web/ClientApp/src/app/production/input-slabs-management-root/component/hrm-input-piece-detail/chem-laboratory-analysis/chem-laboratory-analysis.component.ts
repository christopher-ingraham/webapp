import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Observable } from 'rxjs';

import { LogService } from '@app/core';
import { ComboBoxItemNumberString } from '@app/shared';
import { ChemBaseComponent } from '../view-model-extra';


@Component({
    selector: 'app-chem-laboratory-analysis',
    templateUrl: './chem-laboratory-analysis.component.html',
    styleUrls: ['./chem-laboratory-analysis.component.css']
})
export class ChemLaboratoryAnalysisComponent
    extends ChemBaseComponent
    implements OnInit {

    @Input() public isDefaultSteelGradeUsed: boolean;
    @Output() public isDefaultSteelGradeUsedChange = new EventEmitter<boolean>();

    public samples: ComboBoxItemNumberString[] = [];
    private _selectedSample: ComboBoxItemNumberString;
    public get selectedSample(): ComboBoxItemNumberString {
        return this._selectedSample;
    }
    public set selectedSample(value: ComboBoxItemNumberString) {
        this._selectedSample = value;
        this.sampleIdChange.emit(this._selectedSample.value);    }

    @Input() public set analysisSampleList(obs: Observable<ComboBoxItemNumberString[]>) {
        this.log.debug('ChemLaboratoryAnalysisComponent.analysisSampleList SET');
        obs.subscribe((samples) => {
            this.samples = samples;
            if (samples && samples.length) {
                this.selectedSample = samples[0];
            }
        });
    }

    @Output() public sampleIdChange = new EventEmitter<number>();

    constructor(private log: LogService) {
        super();
    }

    ngOnInit() { }

}
