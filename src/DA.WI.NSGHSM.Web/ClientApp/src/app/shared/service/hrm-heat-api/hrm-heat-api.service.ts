import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService } from '@app/core';
import { BaseEntityService } from '../../base';
import {
    HrmHeat,
    HrmHeatBase,
    HrmHeatDetail,
    HrmHeatForInsert,
    HrmHeatForUpdate,
    HrmHeatListFilter,
    HrmHeatListItem,
} from './models';

@Injectable({
    providedIn: 'root'
})
export class HrmHeatApiService
    extends BaseEntityService<
        HrmHeatBase,
        HrmHeat,
        HrmHeatDetail,
        HrmHeatForInsert,
        HrmHeatForUpdate,
        HrmHeatListItem,
        HrmHeatListFilter> {

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
            'production/HrmHeat',
            'production/HrmHeat/list'
        );
    }

    dtoToClass(data: any): HrmHeatDetail {
        return plainToClass(HrmHeatDetail, data);
    }
    dtoListToClassList(data: any[]): HrmHeat[] {
        return plainToClass(HrmHeatDetail, data);
    }

}
