import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService } from '@app/core';
import { BaseEntityService } from '../../base/base-entity.service';
import {
    TdbProcessCodeBase,
    TdbProcessCode,
    TdbProcessCodeDetail,
    TdbProcessCodeForInsert,
    TdbProcessCodeForUpdate,
    TdbProcessCodeListItem,
    TdbProcessCodeListFilter,
    TdbProcessCodeCodeType,
} from './model';

@Injectable({
    providedIn: 'root'
})
export class TdbProcessCodeApiService
    extends BaseEntityService<
    TdbProcessCodeBase,
    TdbProcessCode,
    TdbProcessCodeDetail,
    TdbProcessCodeForInsert,
    TdbProcessCodeForUpdate,
    TdbProcessCodeListItem,
    TdbProcessCodeListFilter> {

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
            'qcs/TdbProcessCode',
            'qcs/TdbProcessCode/list'
        );
    }

    dtoToClass(data: any): TdbProcessCodeDetail {
        return plainToClass(TdbProcessCodeDetail, data);
    }
    dtoListToClassList(data: any[]): TdbProcessCodeListItem[] {
        return plainToClass(TdbProcessCodeDetail, data);
    }

    public readSourceProcessList() {
        return this.readListByType(TdbProcessCodeCodeType.Source);
    }
    public readDestinationProcessList() {
        return this.readListByType(TdbProcessCodeCodeType.Destination);
    }

    private readListByType(codeType: TdbProcessCodeCodeType) {
        const filter = new TdbProcessCodeListFilter();
        filter.CodeType = codeType;
        const listRequest = this.createListRequest<TdbProcessCodeListFilter>(filter);
        return this.readList(listRequest);
    }
}
