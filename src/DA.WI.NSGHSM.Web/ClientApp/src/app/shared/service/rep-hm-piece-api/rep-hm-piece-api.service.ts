
// rep-hm-piece-api.service.ts
import { Injectable } from '@angular/core';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService } from '@app/core';
import {
    RepHmPiece,
    RepHmPieceBase,
    RepHmPieceDetail,
    RepHmPieceForInsert,
    RepHmPieceForUpdate,
    RepHmPieceListFilter,
    RepHmPieceListItem,
} from './rep-hm-piece.model';
import { BaseEntityService } from '../../base';

@Injectable({
    providedIn: 'root'
})
export class RepHmPieceApiService
    extends BaseEntityService<RepHmPieceBase,
                            RepHmPiece,
                            RepHmPieceDetail,
                            RepHmPieceForInsert,
                            RepHmPieceForUpdate,
                            RepHmPieceListItem,
                            RepHmPieceListFilter> {

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
            'qcs/RepHmPiece',
            'qcs/RepHmPiece/list'
        );
    }

    dtoToClass(data: any): RepHmPieceDetail {
        return plainToClass(RepHmPieceDetail, data);
    }

    dtoListToClassList(data: any[]): RepHmPiece[] {
        return plainToClass(RepHmPiece, data);
    }

}
