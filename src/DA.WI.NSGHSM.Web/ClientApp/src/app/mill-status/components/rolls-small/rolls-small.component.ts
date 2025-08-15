import { Component, OnInit, Input } from '@angular/core';

import { RollssmallData } from '@app/shared';

@Component({
    selector: 'app-rolls-small',
    templateUrl: './rolls-small.component.html',
    styleUrls: ['./rolls-small.component.scss']
})
export class RollsSmallComponent implements OnInit {

    constructor() { }

    @Input()
    public data: RollssmallData;

    ngOnInit() {
    }

}
