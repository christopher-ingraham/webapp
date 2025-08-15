import { Component, OnInit, Input } from '@angular/core';

import { DowncoilerData } from '@app/shared';

@Component({
    selector: 'app-downcoiler',
    templateUrl: './downcoiler.component.html',
    styleUrls: ['./downcoiler.component.scss']
})
export class DowncoilerComponent implements OnInit {

    constructor() { }

    @Input()
    public data: DowncoilerData;

    ngOnInit() {
    }

}
