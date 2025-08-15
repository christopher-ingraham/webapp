import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { UrlHelperService, ApiService, LogService, NotifierService } from '@app/core';

import { BaseEntityService } from '../../base/base-entity.service';

import {
    ShiftReportBase,
    ShiftReport,
    ShiftReportDetail,
    ShiftReportForInsert,
    ShiftReportForUpdate,
    ShiftReportListItem,
    ShiftReportListFilter,
} from './model';

@Injectable({
    providedIn: 'root'
})
export class ShiftReportApiService
    extends BaseEntityService<
    ShiftReportBase,
    ShiftReport,
    ShiftReportDetail,
    ShiftReportForInsert,
    ShiftReportForUpdate,
    ShiftReportListItem,
    ShiftReportListFilter> {

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
            'report/ShiftReport',
            'report/ShiftReport/list'
        );
    }

    dtoToClass(data: any): ShiftReportDetail {
        return plainToClass(ShiftReportDetail, data);
    }

    dtoListToClassList(data: any[]): ShiftReportListItem[] {
        return plainToClass(ShiftReportListItem, data);
    }
}
