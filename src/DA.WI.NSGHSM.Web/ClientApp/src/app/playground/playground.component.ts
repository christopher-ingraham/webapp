import { Component, OnInit } from '@angular/core';
import { FromToDate, HrmInputPieceApiService, ComboBoxItemNumberString } from '@app/shared';

@Component({
    selector: 'app-playground',
    templateUrl: './playground.component.html',
    styleUrls: ['./playground.component.css']
})
export class PlaygroundComponent implements OnInit {

    private _dtFrom: Date;
    public get dtFrom(): Date { return this._dtFrom; }
    public set dtFrom(value: Date) {
        this._dtFrom = value;
    }

    private _dtTo: Date;
    public get dtTo(): Date { return this._dtTo; }
    public set dtTo(value: Date) {
        this._dtTo = value;
    }

    public thisShift = new FromToDate(new Date(2007, 1, 1), new Date(2007, 1, 2));
    public lastShift = new FromToDate(new Date(1970, 2, 2), new Date(1970, 2, 3));

    public hrmInputPieceLookupList: ComboBoxItemNumberString[] = [];

    constructor(private hrmInputPieceApiService: HrmInputPieceApiService) {
        this.dtFrom = new Date();
        this.dtTo = new Date();
    }

    ngOnInit() {
    }

    public toolbarButtonClicked(event: KeyboardEvent, tag: string) {
        alert(`${tag} button clicked`);
    }

    public setPlannedProductionDate(event: Date[]) {
        this.dtFrom = event[0];
        this.dtTo = event[1];
    }

    // how o use BaseApiService.lookup<TId>()
    public lookupHrmInputPiece(event: KeyboardEvent) {
        this.hrmInputPieceApiService.entityLookupList.subscribe((itemList) =>
            this.hrmInputPieceLookupList = itemList
        );
    }
}
