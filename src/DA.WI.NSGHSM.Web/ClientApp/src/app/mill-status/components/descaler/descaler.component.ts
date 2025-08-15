import { Component, OnInit, Input } from '@angular/core';

import { DescalerData } from '@app/shared';

@Component({
    selector: 'app-descaler',
    templateUrl: './descaler.component.html',
    styleUrls: ['./descaler.component.scss']
})
export class DescalerComponent implements OnInit {

    @Input()
    public data: DescalerData;

    constructor() { }

    ngOnInit() {
    }

}
