import { Injectable } from '@angular/core';

import { LogService, ApiService, UrlHelperService, NotifierService } from '@app/core';
import { BaseEntityService } from '../../base/base-entity.service';
import { ComboBoxItemNumberString, ComboBoxItemListBuilder } from '../aux-value';
import {
    TdbMaterialSpecBase,
    TdbMaterialSpec,
    TdbMaterialSpecDetail,
    TdbMaterialSpecForInsert,
    TdbMaterialSpecForUpdate,
    TdbMaterialSpecListItem,
    TdbMaterialSpecListFilter,
} from './model';
import { plainToClass } from 'class-transformer';
import { Observable } from 'rxjs';


@Injectable({
    providedIn: 'root'
})
export class TdbMaterialSpecApiService
    extends BaseEntityService<
    TdbMaterialSpecBase,
    TdbMaterialSpec,
    TdbMaterialSpecDetail,
    TdbMaterialSpecForInsert,
    TdbMaterialSpecForUpdate,
    TdbMaterialSpecListItem,
    TdbMaterialSpecListFilter> {

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
            'production/TdbMaterialSpec/lookup'
        );
    }

    dtoToClass(data: any): TdbMaterialSpecDetail {
        return plainToClass(TdbMaterialSpecDetail, data);
    }
    dtoListToClassList(data: any[]): TdbMaterialSpecListItem[] {
        return plainToClass(TdbMaterialSpecDetail, data);
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
