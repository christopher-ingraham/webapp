import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService } from '@app/core';
import { BaseEntityService } from '@app/shared';
import {
    Measurement,
    MeasurementBase,
    MeasurementDetail,
    MeasurementForInsert,
    MeasurementForUpdate,
    MeasurementListFilter,
    MeasurementListItem,
} from './measurement.model';

@Injectable({
    providedIn: 'root'
})
export class MeasurementApiService
    extends BaseEntityService<MeasurementBase,
                            Measurement,
                            MeasurementDetail,
                            MeasurementForInsert,
                            MeasurementForUpdate,
                            MeasurementListItem,
                            MeasurementListFilter> {

    constructor(
        log: LogService,
        notifier: NotifierService,
        apiService: ApiService,
        urlHelperService: UrlHelperService
    ) {
        super(log, notifier, apiService, urlHelperService, 'example/measurements');
    }

    dtoToClass(data: any): MeasurementDetail {
        return plainToClass(MeasurementDetail, data);
    }

    dtoListToClassList(data: any[]): Measurement[] {
        return plainToClass(Measurement, data);
    }

}
