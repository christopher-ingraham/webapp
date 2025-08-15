import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService } from '@app/core';
import { BaseEntityService } from '../../base';
import {
    TdbMaterialGradeBase,
    TdbMaterialGrade,
    TdbMaterialGradeDetail,
    TdbMaterialGradeForInsert,
    TdbMaterialGradeForUpdate,
    TdbMaterialGradeListItem,
    TdbMaterialGradeListFilter,
} from './model';

@Injectable({
  providedIn: 'root'
})
export class TdbMaterialGradeApiService
    extends BaseEntityService<
    TdbMaterialGradeBase,
    TdbMaterialGrade,
    TdbMaterialGradeDetail,
    TdbMaterialGradeForInsert,
    TdbMaterialGradeForUpdate,
    TdbMaterialGradeListItem,
    TdbMaterialGradeListFilter> {

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
            'qcs/TdbMaterialGrade',
            'qcs/TdbMaterialGrade/list'
        );
    }

    dtoToClass(data: any): TdbMaterialGradeDetail {
        return plainToClass(TdbMaterialGradeDetail, data);
    }
    dtoListToClassList(data: any[]): TdbMaterialGradeListItem[] {
        return plainToClass(TdbMaterialGradeDetail, data);
    }
}
