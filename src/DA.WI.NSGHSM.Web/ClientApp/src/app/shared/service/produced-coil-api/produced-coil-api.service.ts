import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { plainToClass } from 'class-transformer';

import { LogService, ApiService, UrlHelperService, NotifierService } from '@app/core';

import { BaseEntityService } from '../../base';
import {
    ProducedCoil,
    ProducedCoilBase,
    ProducedCoilDetail,
    ProducedCoilForInsert,
    ProducedCoilForUpdate,
    ProducedCoilListFilter,
    ProducedCoilListItem,
} from './model';
import { ComboBoxItemNumberString, ComboBoxItemStringString, ComboBoxItemListBuilder } from '../aux-value';

@Injectable({
    providedIn: 'root'
})
export class ProducedCoilApiService
    extends BaseEntityService<
    ProducedCoilBase,
    ProducedCoil,
    ProducedCoilDetail,
    ProducedCoilForInsert,
    ProducedCoilForUpdate,
    ProducedCoilListItem,
    ProducedCoilListFilter> {

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
            'production/ProducedCoils',
            'production/ProducedCoils/list',
            'production/ProducedCoils/lookup',
            'production/ProducedCoils/validation'
        );
    }

    dtoToClass(data: any): ProducedCoilDetail {
        return plainToClass(ProducedCoilDetail, data);
    }

    dtoListToClassList(data: any[]): ProducedCoil[] {
        return plainToClass(ProducedCoil, data);
    }

    public get inputSlabList(): Observable<ComboBoxItemNumberString[]> {
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
