import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'app-audit-info',
    templateUrl: './audit-info.component.html',
    styleUrls: ['./audit-info.component.css']
})
export class AuditInfoComponent implements OnInit {

    @Input() public operatorLabel = 'Operator';
    @Input() public operator: string;

    @Input() public revisionLabel = 'Revision';
    @Input() public revision: Date;

    @Input() public title = 'Audit';

    @Input() public disabled = true;

    public visible = false;

    constructor() { }

    ngOnInit() {
        setTimeout(() => this.visible = true, 1000);
    }

}
