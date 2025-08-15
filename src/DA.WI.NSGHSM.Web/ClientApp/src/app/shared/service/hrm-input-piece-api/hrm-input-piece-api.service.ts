
// hrm-input-piece-api.service.ts
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService } from '@app/core';
import {
    HrmInputPieceListFilter,
    HrmInputPiece,
    HrmInputPieceBase,
    HrmInputPieceDetail,
    HrmInputPieceForInsert,
    HrmInputPieceForUpdate,
    HrmInputPieceListItem,
} from './models';
import { BaseEntityService } from '../../base';
import { ComboBoxItemNumberString, ComboBoxItemListBuilder } from '../aux-value';

@Injectable({
    providedIn: 'root'
})
export class HrmInputPieceApiService
    extends BaseEntityService<
        HrmInputPieceBase,
        HrmInputPiece,
        HrmInputPieceDetail,
        HrmInputPieceForInsert,
        HrmInputPieceForUpdate,
        HrmInputPieceListItem,
        HrmInputPieceListFilter> {

    constructor(
        log: LogService,
        notifier: NotifierService,
        apiService: ApiService,
        urlHelperService: UrlHelperService
    ) {
        super(
            log,
            notifier,
            apiService,
            urlHelperService,
            'production/HrmInputPiece',
            'production/HrmInputPiece/list',
            'production/HrmInputPiece/lookup',
            'production/HrmInputPiece/validation',
        );
    }

    dtoToClass(data: any): HrmInputPieceDetail {
        return plainToClass(HrmInputPieceDetail, data);
    }

    dtoListToClassList(data: any[]): HrmInputPieceListItem[] {
        return plainToClass(HrmInputPieceListItem, data);
    }

    public get entityLookupList(): Observable<ComboBoxItemNumberString[]> {
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.lookupNumber().subscribe((listResult) => {
                if (listResult.total) {
                    const items = ComboBoxItemListBuilder.build(listResult.data, ComboBoxItemNumberString, 'value', 'display');
                    obs.next(items);
                } else {
                    obs.next([]);
                }
            });
        });
    }

}
