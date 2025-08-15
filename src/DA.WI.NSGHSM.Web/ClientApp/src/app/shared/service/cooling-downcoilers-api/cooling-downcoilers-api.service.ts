import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { UrlHelperService, ApiService, LogService, NotifierService } from '@app/core';

import { BaseEntityService } from '../../base/base-entity.service';

import { CoolingDowncoilersData } from './model';

@Injectable({
    providedIn: 'root'
})
export class CoolingDowncoilersApiService
    extends BaseEntityService<
    CoolingDowncoilersData,
    CoolingDowncoilersData,
    CoolingDowncoilersData,
    CoolingDowncoilersData,
    CoolingDowncoilersData,
    CoolingDowncoilersData,
    CoolingDowncoilersData> {

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
            'plantoverview/CoolingDowncoilersData',
            'plantoverview/CoolingDowncoilersData/list'
        );
    }

    dtoToClass(data: any): CoolingDowncoilersData {
        return plainToClass(CoolingDowncoilersData, data);
    }
    dtoListToClassList(data: any[]): CoolingDowncoilersData[] {
        throw new Error('Method not implemented.');
    }
}
