import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { ChemRowData, ChemBaseComponent } from '../view-model-extra';

@Component({
    selector: 'app-chem-material-specification',
    templateUrl: './chem-material-specification.component.html',
    styleUrls: ['./chem-material-specification.component.css']
})
export class ChemMaterialSpecificationComponent
    extends ChemBaseComponent
    implements OnInit {

    constructor() {
        super();
    }

    ngOnInit() {
    }

}
