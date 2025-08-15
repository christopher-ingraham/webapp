import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { UrlHelperService, ApiService, LogService, NotifierService } from '@app/core';

import { BaseEntityService } from '../../base/base-entity.service';

import { RoughingMillData } from './model/roughing-mill-data.class';

@Injectable({
    providedIn: 'root'
})
export class RoughingMillApiService
    extends BaseEntityService<
    RoughingMillData,
    RoughingMillData,
    RoughingMillData,
    RoughingMillData,
    RoughingMillData,
    RoughingMillData,
    RoughingMillData> {

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
            'plantoverview/RoughingMillData',
            'plantoverview/RoughingMillData/list'
        );
    }

    dtoToClass(data: any): RoughingMillData {
        return plainToClass(RoughingMillData, data);
    }
    dtoListToClassList(data: any[]): RoughingMillData[] {
        throw new Error('Method not implemented.');
    }
}
