import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { ChemRowData } from '../view-model-extra';

export abstract class ChemBaseComponent {

    @Input() public modelOptions: any = {};

    @Input() public chemElement: any;
    @Input() public form: FormGroup;
    @Input() public items: ChemRowData[];

}
