import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { UrlHelperService, ApiService, LogService, NotifierService } from '@app/core';

import { BaseEntityService } from '../../base/base-entity.service';

import {
    StoppageReportBase,
    StoppageReport,
    StoppageReportDetail,
    StoppageReportForInsert,
    StoppageReportForUpdate,
    StoppageReportListItem,
    StoppageReportListFilter,
} from './model';

@Injectable({
    providedIn: 'root'
})
export class StoppageReportApiService
    extends BaseEntityService<
    StoppageReportBase,
    StoppageReport,
    StoppageReportDetail,
    StoppageReportForInsert,
    StoppageReportForUpdate,
    StoppageReportListItem,
    StoppageReportListFilter> {

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
            'report/StoppageReport',
            'report/StoppageReport/list'
        );
    }

    dtoToClass(data: any): StoppageReportDetail {
        return plainToClass(StoppageReportDetail, data);
    }

    dtoListToClassList(data: any[]): StoppageReportListItem[] {
        return plainToClass(StoppageReportListItem, data);
    }
}
