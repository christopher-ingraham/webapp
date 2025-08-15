import { Component, OnInit, Input } from '@angular/core';

import { StandData } from '@app/shared';

@Component({
    selector: 'app-stand',
    templateUrl: './stand.component.html',
    styleUrls: ['./stand.component.scss']
})
export class StandComponent implements OnInit {

    @Input()
    public data: StandData;

    constructor() { }

    ngOnInit() {
    }

}
