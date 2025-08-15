import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService } from '@app/core';
import { BaseEntityService } from '../../base';
import {
    RepHmPieceStat,
    RepHmPieceStatBase,
    RepHmPieceStatDetail,
    RepHmPieceStatForInsert,
    RepHmPieceStatForUpdate,
    RepHmPieceStatListFilter,
    RepHmPieceStatListItem,
} from './model';

@Injectable({
    providedIn: 'root'
})
export class RepHmPieceStatApiService
    extends BaseEntityService<
    RepHmPieceStatBase,
    RepHmPieceStat,
    RepHmPieceStatDetail,
    RepHmPieceStatForInsert,
    RepHmPieceStatForUpdate,
    RepHmPieceStatListItem,
    RepHmPieceStatListFilter> {

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
            'qcs/RepHmPieceStat',
            'qcs/RepHmPieceStat/list'
        );
    }

    dtoToClass(data: any): RepHmPieceStatDetail {
        return plainToClass(RepHmPieceStatDetail, data);
    }
    dtoListToClassList(data: any[]): RepHmPieceStatListItem[] {
        return plainToClass(RepHmPieceStatDetail, data);
    }
}
