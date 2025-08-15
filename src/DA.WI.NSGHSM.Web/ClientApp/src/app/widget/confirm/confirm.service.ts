import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import {
    DialogCloseResult,
    DialogRef,
    DialogResult,
    DialogService,
    DialogSettings,
} from '@progress/kendo-angular-dialog';

import { ConfirmResult } from './confirm-result.class';

@Injectable({
    providedIn: 'root'
})
export class ConfirmService {

    public ActionTextYes = 'Yes';
    public ActionTextNo = 'No';
    public ActionTextCancel = 'Cancel';

    constructor(private dialogService: DialogService) { }

    public open(settings: DialogSettings): Observable<ConfirmResult> {
        return new Observable<ConfirmResult>((obs) => {
            const dialog: DialogRef = this.dialogService.open(settings);

            dialog.result.subscribe(
                (result: DialogResult) => {
                    if (result instanceof DialogCloseResult) {
                        obs.next(ConfirmResult.createClose());
                    } else {
                        obs.next(ConfirmResult.createAction(result));
                    }
                },
                (error) => {
                    obs.next(ConfirmResult.createError(error));
                }
            );
        });
    }

    public openYesNo(title: string, question: string): Observable<ConfirmResult> {
        const settings: DialogSettings = {
            title,
            content: question,
            actions: [
                { text: this.ActionTextNo },
                { text: this.ActionTextYes, primary: true }
            ],
            width: 450,
            height: 200,
            minWidth: 250
        };
        return this.open(settings);
    }

    public openYesNoCancel(title: string, question: string): Observable<ConfirmResult> {
        const settings: DialogSettings = {
            title,
            content: question,
            actions: [
                { text: this.ActionTextCancel },
                { text: this.ActionTextNo },
                { text: this.ActionTextYes, primary: true }
            ],
            width: 450,
            height: 200,
            minWidth: 250
        };
        return this.open(settings);
    }

}
