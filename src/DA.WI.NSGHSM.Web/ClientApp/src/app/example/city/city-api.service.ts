import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService } from '@app/core';
import { BaseEntityService } from '@app/shared';
import {
    City,
    CityBase,
    CityDetail,
    CityForInsert,
    CityForUpdate,
    CityListFilter,
    CityListItem,
} from './city.model';

@Injectable({
    providedIn: 'root'
})
export class CityApiService
    extends BaseEntityService<CityBase,
                            City,
                            CityDetail,
                            CityForInsert,
                            CityForUpdate,
                            CityListItem,
                            CityListFilter> {

    constructor(
        log: LogService,
        notifier: NotifierService,
        apiService: ApiService,
        urlHelperService: UrlHelperService
    ) {
        super(log, notifier, apiService, urlHelperService, 'example/cities');
    }

    dtoToClass(data: any): CityDetail {
        return plainToClass(CityDetail, data);
    }

    dtoListToClassList(data: any[]): CityListItem[] {
        return plainToClass(City, data);
    }

}
