import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService } from '@app/core';
import {
    RepHmTrendsView,
    RepHmTrendsViewBase,
    RepHmTrendsViewDetail,
    RepHmTrendsViewForInsert,
    RepHmTrendsViewForUpdate,
    RepHmTrendsViewListFilter,
    RepHmTrendsViewListItem,
} from './rep-hm-trends-view.model';
import { BaseEntityService } from '../../base';

@Injectable({
    providedIn: 'root'
})
export class RepHmTrendsViewApiService
    extends BaseEntityService<
        RepHmTrendsViewBase,
        RepHmTrendsView,
        RepHmTrendsViewDetail,
        RepHmTrendsViewForInsert,
        RepHmTrendsViewForUpdate,
        RepHmTrendsViewListItem,
        RepHmTrendsViewListFilter> {

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
            'qcs/RepHmTrendsView',
            'qcs/RepHmTrendsView/list'
        );
    }

    dtoToClass(data: any): RepHmTrendsViewDetail {
        return plainToClass(RepHmTrendsViewDetail, data);
    }

    dtoListToClassList(data: any[]): RepHmTrendsViewListItem[] {
        return plainToClass(RepHmTrendsView, data);
    }

}
