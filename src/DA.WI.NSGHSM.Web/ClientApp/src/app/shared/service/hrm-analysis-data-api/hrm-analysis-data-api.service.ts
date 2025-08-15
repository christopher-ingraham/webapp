import { Injectable } from '@angular/core';

import { ApiService, UrlHelperService, LogService, NotifierService, } from '@app/core';
import { BaseEntityService } from '../../base';
import { ComboBoxItemNumberString } from '../aux-value';
import {
    HrmAnalysisDataBase,
    HrmAnalysisData,
    HrmAnalysisDataDetail,
    HrmAnalysisDataForInsert,
    HrmAnalysisDataForUpdate,
    HrmAnalysisDataListFilter,
    HrmAnalysisDataListItem,
} from './models';
import { plainToClass } from 'class-transformer';

@Injectable({
  providedIn: 'root'
})
export class HrmAnalysisDataApiService
    extends BaseEntityService<
        HrmAnalysisDataBase,
        HrmAnalysisData,
        HrmAnalysisDataDetail,
        HrmAnalysisDataForInsert,
        HrmAnalysisDataForUpdate,
        HrmAnalysisDataListItem,
        HrmAnalysisDataListFilter> {

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
            'production/HrmAnalysisData',
            'production/HrmAnalysisData/list');
    }

    dtoToClass(data: any): HrmAnalysisDataDetail {
        return plainToClass(HrmAnalysisDataDetail, data);
    }
    dtoListToClassList(data: any[]): HrmAnalysisData[] {
        return plainToClass(HrmAnalysisDataDetail, data);
    }

}
