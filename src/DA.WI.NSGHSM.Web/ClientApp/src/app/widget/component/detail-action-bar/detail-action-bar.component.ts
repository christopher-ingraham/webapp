import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'app-detail-action-bar',
    templateUrl: './detail-action-bar.component.html',
    styleUrls: ['./detail-action-bar.component.css']
})
export class DetailActionBarComponent implements OnInit {
    @Input() public disabled: boolean;
    @Input() public isNew: boolean;
    @Input() public isReadOnly: boolean;

    @Input() public deleteLabel: string = 'Delete';
    @Input() public deleteVisible = true;
    @Output() public deletePressed = new EventEmitter<KeyboardEvent>();

    @Input() public saveLabel: string = 'Save';
    @Input() public saveVisible = true;
    @Output() public savePressed = new EventEmitter<KeyboardEvent>();

    @Input() public cancelLabel: string = 'Cancel';
    @Input() public cancelVisible = true;
    @Output() public cancelPressed = new EventEmitter<KeyboardEvent>();

    public get layoutAlign(): string {
        return this.isNew ? 'flex-end' : 'space-between';
    }

    constructor() { }

    ngOnInit() {
    }

}
