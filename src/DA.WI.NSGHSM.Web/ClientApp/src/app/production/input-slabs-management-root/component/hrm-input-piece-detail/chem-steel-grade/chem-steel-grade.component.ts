import { Component, OnInit, Input } from '@angular/core';

import { ChemBaseComponent } from '../view-model-extra';

@Component({
    selector: 'app-chem-steel-grade',
    templateUrl: './chem-steel-grade.component.html',
    styleUrls: ['./chem-steel-grade.component.css']
})
export class ChemSteelGradeComponent
    extends ChemBaseComponent
    implements OnInit {

    constructor() {
        super();
    }

    ngOnInit() { }

}
