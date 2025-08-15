import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, NotifierService, } from '@app/core';
import { BaseEntityService } from '../../base';

import {
    TdbAlloySpec,
    TdbAlloySpecBase,
    TdbAlloySpecDetail,
    TdbAlloySpecForInsert,
    TdbAlloySpecForUpdate,
    TdbAlloySpecListFilter,
    TdbAlloySpecListItem,
} from './models';

@Injectable({
    providedIn: 'root'
})
export class TdbAlloySpecApiService
    extends BaseEntityService<
        TdbAlloySpecBase,
        TdbAlloySpec,
        TdbAlloySpecDetail,
        TdbAlloySpecForInsert,
        TdbAlloySpecForUpdate,
        TdbAlloySpecListItem,
        TdbAlloySpecListFilter> {

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
            'qcs/TdbAlloySpec',
            'qcs/TdbAlloySpec/list');
    }

    dtoToClass(data: any): TdbAlloySpecDetail {
        return plainToClass(TdbAlloySpecDetail, data);
    }
    dtoListToClassList(data: any[]): TdbAlloySpecDetail[] {
        return plainToClass(TdbAlloySpecDetail, data);
    }

    public codeToCnt(code: string): Observable<number> {
        return new Observable<number>((obs) => {
            const filter = new TdbAlloySpecListFilter();
            const listRequest = this.createListRequest<TdbAlloySpecListFilter>(filter);
            this.readList(listRequest).subscribe(
                (listResult) => {
                    if (listResult.total > 0) {
                        const candidates = listResult.data.filter((li) => (li.alloySpecCode === code));
                        if (candidates.length > 0) {
                            obs.next(candidates[0].alloySpecCnt);
                        } else {
                            obs.error(`${TdbAlloySpecApiService['name']}.codeToCnt(code="${code}"): not found`);
                        }
                    }
                },
                (readListError) => obs.error(readListError)
            );
        });
    }
}
