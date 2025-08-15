import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, ListResult, ListRequest, NotifierService } from '@app/core';
import { BaseEntityService } from '../../base';
import {
    RepHmSetup,
    RepHmSetupBase,
    RepHmSetupDetail,
    RepHmSetupForInsert,
    RepHmSetupForUpdate,
    RepHmSetupListFilter,
    RepHmSetupListItem,
} from './model';

@Injectable({
    providedIn: 'root'
})
export class RepHmSetupApiService
    extends BaseEntityService<
        RepHmSetupBase,
        RepHmSetup,
        RepHmSetupDetail,
        RepHmSetupForInsert,
        RepHmSetupForUpdate,
        RepHmSetupListItem,
        RepHmSetupListFilter> {

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
            'production/RepHmSetup/list'
        );
    }

    dtoToClass(data: any): RepHmSetupDetail {
        return plainToClass(RepHmSetupDetail, data);
    }

    dtoListToClassList(data: any[]): RepHmSetup[] {
        return plainToClass(RepHmSetup, data);
    }
}
