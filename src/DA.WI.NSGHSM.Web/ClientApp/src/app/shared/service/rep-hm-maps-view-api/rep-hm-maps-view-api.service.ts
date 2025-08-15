import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService } from '@app/core';
import {
    RepHmMapsView,
    RepHmMapsViewBase,
    RepHmMapsViewDetail,
    RepHmMapsViewForInsert,
    RepHmMapsViewForUpdate,
    RepHmMapsViewListFilter,
    RepHmMapsViewListItem,
} from './rep-hm-maps-view.model';
import { BaseEntityService } from '../../base';

@Injectable({
    providedIn: 'root'
})
export class RepHmMapsViewApiService
    extends BaseEntityService<
    RepHmMapsViewBase,
    RepHmMapsView,
    RepHmMapsViewDetail,
    RepHmMapsViewForInsert,
    RepHmMapsViewForUpdate,
    RepHmMapsViewListItem,
    RepHmMapsViewListFilter> {

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
            'qcs/RepHmMapsView',
            'qcs/RepHmMapsView/list'
        );
    }

    dtoToClass(data: any): RepHmMapsViewDetail {
        return plainToClass(RepHmMapsViewDetail, data);
    }

    dtoListToClassList(data: any[]): RepHmMapsViewListItem[] {
        return plainToClass(RepHmMapsView, data);
    }

}
