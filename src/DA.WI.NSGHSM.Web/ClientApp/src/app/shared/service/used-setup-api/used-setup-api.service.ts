import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService, } from '@app/core';
import { BaseEntityService } from '../../base/base-entity.service';
import {
    UsedSetup,
    UsedSetupBase,
    UsedSetupDetail,
    UsedSetupForInsert,
    UsedSetupForUpdate,
    UsedSetupListFilter,
    UsedSetupListItem,
} from './model';

@Injectable({
    providedIn: 'root'
})
export class UsedSetupApiService
    extends BaseEntityService<
    UsedSetupBase,
    UsedSetup,
    UsedSetupDetail,
    UsedSetupForInsert,
    UsedSetupForUpdate,
    UsedSetupListItem,
    UsedSetupListFilter> {

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
            'production/RepHmSetup',
            'production/UsedSetup/list'
        );
    }

    dtoToClass(data: any): UsedSetupDetail {
        return plainToClass(UsedSetupDetail, data);
    }

    dtoListToClassList(data: any[]): UsedSetup[] {
        return plainToClass(UsedSetup, data);
    }
}
