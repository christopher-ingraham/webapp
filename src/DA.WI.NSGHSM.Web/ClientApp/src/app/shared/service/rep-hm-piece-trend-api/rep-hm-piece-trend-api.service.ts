
// rep-hm-piece-Trend-api.service.ts
import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService } from '@app/core';
import {
    RepHmPieceTrend,
    RepHmPieceTrendBase,
    RepHmPieceTrendDetail,
    RepHmPieceTrendForInsert,
    RepHmPieceTrendForUpdate,
    RepHmPieceTrendListFilter,
    RepHmPieceTrendListItem,
} from './rep-hm-piece-trend.model';
import { BaseEntityService } from '../../base';

@Injectable({
    providedIn: 'root'
})
export class RepHmPieceTrendApiService
    extends BaseEntityService<
        RepHmPieceTrendBase,
        RepHmPieceTrend,
        RepHmPieceTrendDetail,
        RepHmPieceTrendForInsert,
        RepHmPieceTrendForUpdate,
        RepHmPieceTrendListItem,
        RepHmPieceTrendListFilter> {

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
            'qcs/RepHmPieceTrend',
            'qcs/RepHmPieceTrend/list'
        );
    }

    dtoToClass(data: any): RepHmPieceTrendDetail {
        return plainToClass(RepHmPieceTrendDetail, data);
    }

    dtoListToClassList(data: any[]): RepHmPieceTrendListItem[] {
        return plainToClass(RepHmPieceTrend, data);
    }

}
