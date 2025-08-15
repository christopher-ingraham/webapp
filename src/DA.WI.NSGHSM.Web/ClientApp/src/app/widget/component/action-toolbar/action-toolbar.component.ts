import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { AuthService } from '@app/core';

@Component({
    selector: 'app-action-toolbar',
    templateUrl: './action-toolbar.component.html',
    styleUrls: ['./action-toolbar.component.css'],
})
export class ActionToolbarComponent implements OnInit {

    @Input() printVisible = false;
    @Input() printDisabled = false;
    @Output() printClicked = new EventEmitter<KeyboardEvent>();

    @Input() newVisible;
    @Input() newDisabled = false;
    @Output() newClicked = new EventEmitter<KeyboardEvent>();

    @Input() removeVisible;
    @Input() removeDisabled = false;
    @Output() removeClicked = new EventEmitter<KeyboardEvent>();

    @Input() editVisible;
    @Input() editDisabled = false;
    @Output() editClicked = new EventEmitter<KeyboardEvent>();

    @Input() copyVisible;
    @Input() copyDisabled = false;
    @Output() copyClicked = new EventEmitter<KeyboardEvent>();

    @Input() viewVisible = false;
    @Input() viewDisabled = false;
    @Output() viewClicked = new EventEmitter<KeyboardEvent>();

    @Input() refreshVisible = false;
    @Input() refreshDisabled = false;
    @Output() refreshClicked = new EventEmitter<KeyboardEvent>();

    @Input() saveVisible = false;
    @Input() saveDisabled = false;
    @Output() saveClicked = new EventEmitter<KeyboardEvent>();

    @Input() exportAllDisabled = false;
    @Output() exportAllClicked = new EventEmitter<KeyboardEvent>();

    @Input() look = undefined; // 'flat' | 'outline'

    constructor(private authService: AuthService) {
    }

    ngOnInit() { }


}
