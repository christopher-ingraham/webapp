import { Component, OnInit, Input } from '@angular/core';

import { TrimmingData } from '@app/shared';

@Component({
    selector: 'app-trimming',
    templateUrl: './trimming.component.html',
    styleUrls: ['./trimming.component.scss']
})
export class TrimmingComponent implements OnInit {

    constructor() { }

    @Input()
    public data: TrimmingData;

    ngOnInit() {
    }

}
