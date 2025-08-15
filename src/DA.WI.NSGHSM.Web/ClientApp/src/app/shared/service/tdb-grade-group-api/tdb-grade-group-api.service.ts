import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService } from '@app/core';
import { BaseEntityService } from '../../base';
import {
    TdbGradeGroupBase,
    TdbGradeGroup,
    TdbGradeGroupDetail,
    TdbGradeGroupForInsert,
    TdbGradeGroupListItem,
    TdbGradeGroupForUpdate,
    TdbGradeGroupListFilter,
} from './model';

@Injectable({
  providedIn: 'root'
})
export class TdbGradeGroupApiService
    extends BaseEntityService<
    TdbGradeGroupBase,
    TdbGradeGroup,
    TdbGradeGroupDetail,
    TdbGradeGroupForInsert,
    TdbGradeGroupForUpdate,
    TdbGradeGroupListItem,
    TdbGradeGroupListFilter> {

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
            'qcs/TdbGradeGroup',
            'qcs/TdbGradeGroup/list'
        );
    }

    dtoToClass(data: any): TdbGradeGroupDetail {
        return plainToClass(TdbGradeGroupDetail, data);
    }
    dtoListToClassList(data: any[]): TdbGradeGroupListItem[] {
        return plainToClass(TdbGradeGroupDetail, data);
    }
}
