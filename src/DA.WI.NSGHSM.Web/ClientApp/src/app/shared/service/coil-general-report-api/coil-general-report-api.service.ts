import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { UrlHelperService, ApiService, LogService, NotifierService } from '@app/core';

import { BaseEntityService } from '../../base/base-entity.service';

import {
    CoilGeneralReport,
    CoilGeneralReportBase,
    CoilGeneralReportDetail,
    CoilGeneralReportForInsert,
    CoilGeneralReportForUpdate,
    CoilGeneralReportListFilter,
    CoilGeneralReportListItem,
} from './model';

@Injectable({
    providedIn: 'root'
})
export class CoilGeneralReportApiService
    extends BaseEntityService<
    CoilGeneralReportBase,
    CoilGeneralReport,
    CoilGeneralReportDetail,
    CoilGeneralReportForInsert,
    CoilGeneralReportForUpdate,
    CoilGeneralReportListItem,
    CoilGeneralReportListFilter> {

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
            'report/CoilGeneralReport',
            'report/CoilGeneralReport/list'
        );
    }

    dtoToClass(data: any): CoilGeneralReportDetail {
        return plainToClass(CoilGeneralReportDetail, data);
    }

    dtoListToClassList(data: any[]): CoilGeneralReportListItem[] {
        return plainToClass(CoilGeneralReportListItem, data);
    }
}
