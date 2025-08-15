import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, ListResult, ListRequest, NotifierService } from '@app/core';
import { BaseEntityService } from '../../base';
import {
    ExitSaddle,
    ExitSaddleBase,
    ExitSaddleDetail,
    ExitSaddleForInsert,
    ExitSaddleForUpdate,
    ExitSaddleListFilter,
    ExitSaddleListItem,
} from './model';

@Injectable({
    providedIn: 'root'
})
export class ExitSaddleApiService
    extends BaseEntityService<
    ExitSaddleBase,
    ExitSaddle,
    ExitSaddleDetail,
    ExitSaddleForInsert,
    ExitSaddleForUpdate,
    ExitSaddleListItem,
    ExitSaddleListFilter> {

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
            'production/ExitSaddles',
            'production/ExitSaddles/list'
        );
    }

    dtoToClass(data: any): ExitSaddleDetail {
        return plainToClass(ExitSaddleDetail, data);
    }

    dtoListToClassList(data: any[]): ExitSaddle[] {
        return plainToClass(ExitSaddle, data);
    }
}
