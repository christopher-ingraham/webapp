import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { UrlHelperService, ApiService, LogService, NotifierService } from '@app/core';

import { BaseEntityService } from '../../base/base-entity.service';

import {
    PracticeReport,
    PracticeReportBase,
    PracticeReportDetail,
    PracticeReportForInsert,
    PracticeReportForUpdate,
    PracticeReportListFilter,
    PracticeReportListItem,
} from './model';

@Injectable({
    providedIn: 'root'
})
export class PracticeReportApiService
    extends BaseEntityService<
    PracticeReportBase,
    PracticeReport,
    PracticeReportDetail,
    PracticeReportForInsert,
    PracticeReportForUpdate,
    PracticeReportListItem,
    PracticeReportListFilter> {

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
            'report/PracticeReport',
            'report/PracticeReport/list'
        );
    }

    dtoToClass(data: any): PracticeReportDetail {
        return plainToClass(PracticeReportDetail, data);
    }

    dtoListToClassList(data: any[]): PracticeReportListItem[] {
        return plainToClass(PracticeReportListItem, data);
    }
}
