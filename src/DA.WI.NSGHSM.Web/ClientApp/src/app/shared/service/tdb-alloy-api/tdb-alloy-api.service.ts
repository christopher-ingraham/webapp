import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService, } from '@app/core';
import { BaseEntityService } from '../../base';

import {
    TdbAlloy,
    TdbAlloyBase,
    TdbAlloyDetail,
    TdbAlloyForInsert,
    TdbAlloyForUpdate,
    TdbAlloyListFilter,
    TdbAlloyListItem,
} from './models';

@Injectable({
    providedIn: 'root'
})
export class TdbAlloyApiService
    extends BaseEntityService<
    TdbAlloyBase,
    TdbAlloy,
    TdbAlloyDetail,
    TdbAlloyForInsert,
    TdbAlloyForUpdate,
    TdbAlloyListItem,
    TdbAlloyListFilter
    > {

    constructor(
        logService: LogService,
        notifierService: NotifierService,
        apiService: ApiService,
        urlHelperService: UrlHelperService
    ) {
        super(
            logService,
            notifierService,
            apiService,
            urlHelperService,
            'qcs/TdbAlloy',
            'qcs/TdbAlloy/list');
    }

    dtoToClass(data: any): TdbAlloyDetail {
        return plainToClass(TdbAlloyDetail, data);
    }
    dtoListToClassList(data: any[]): TdbAlloyDetail[] {
        return plainToClass(TdbAlloyDetail, data);
    }

}
