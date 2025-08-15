import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { UrlHelperService, ApiService, LogService, NotifierService } from '@app/core';

import { BaseEntityService } from '../../base/base-entity.service';

import { FinishingMillData } from './model/finishing-mill-data.class';

@Injectable({
    providedIn: 'root'
})
export class FinishingMillApiService
    extends BaseEntityService<
    FinishingMillData,
    FinishingMillData,
    FinishingMillData,
    FinishingMillData,
    FinishingMillData,
    FinishingMillData,
    FinishingMillData> {

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
            'plantoverview/FinishingMillData',
            'plantoverview/FinishingMillData/list'
        );
    }

    dtoToClass(data: any): FinishingMillData {
        return plainToClass(FinishingMillData, data);
    }
    dtoListToClassList(data: any[]): FinishingMillData[] {
        throw new Error('Method not implemented.');
    }
}
