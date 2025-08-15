import { Component, OnInit, Input } from '@angular/core';

import { IntensiveData } from '@app/shared';

@Component({
    selector: 'app-intensive',
    templateUrl: './intensive.component.html',
    styleUrls: ['./intensive.component.scss']
})
export class IntensiveComponent implements OnInit {

    constructor() { }

    @Input()
    public data: IntensiveData;

    ngOnInit() {
    }

}
