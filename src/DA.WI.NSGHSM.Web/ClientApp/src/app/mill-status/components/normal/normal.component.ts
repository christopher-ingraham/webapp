import { Component, OnInit, Input } from '@angular/core';

import { NormalData } from '@app/shared';

@Component({
    selector: 'app-normal',
    templateUrl: './normal.component.html',
    styleUrls: ['./normal.component.scss']
})
export class NormalComponent implements OnInit {

    constructor() { }

    @Input()
    public data: NormalData;

    ngOnInit() {
    }

}
