import { Component, OnInit, Input } from '@angular/core';

import { EdgerData} from '@app/shared';

@Component({
    selector: 'app-edger',
    templateUrl: './edger.component.html',
    styleUrls: ['./edger.component.scss']
})
export class EdgerComponent implements OnInit {

    @Input()
    public data: EdgerData;

    constructor() { }

    ngOnInit() {
    }

}
