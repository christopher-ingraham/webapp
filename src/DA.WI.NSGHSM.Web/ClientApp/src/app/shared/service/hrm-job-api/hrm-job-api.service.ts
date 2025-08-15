
// hrm-job-api.service.ts
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { plainToClass } from 'class-transformer';

import { ApiService, UrlHelperService, LogService, ListResult, ListRequest, NotifierService } from '@app/core';
import { BaseEntityService } from '../../base';
import {
    HrmJob,
    HrmJobBase,
    HrmJobDetail,
    HrmJobForInsert,
    HrmJobForUpdate,
    HrmJobListFilter,
    HrmJobListItem,
} from './models';
import { ComboBoxItemStringString, ComboBoxItemListBuilder } from '../aux-value';

@Injectable({
    providedIn: 'root'
})
export class HrmJobApiService
    extends BaseEntityService<
        HrmJobBase,
        HrmJob,
        HrmJobDetail,
        HrmJobForInsert,
        HrmJobForUpdate,
        HrmJobListItem,
        HrmJobListFilter> {

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
            'production/HrmJob',
            'production/HrmJob/list',
            'production/HrmJob/lookup',
        );
    }

    dtoToClass(data: any): HrmJobDetail {
        return plainToClass(HrmJobDetail, data);
    }

    dtoListToClassList(data: any[]): HrmJob[] {
        return plainToClass(HrmJob, data);
    }

    public get nextHrmJobSequence(): Observable<number> {
        return new Observable<number>((obs) => {
            const filter = new HrmJobListFilter();
            const listRequest = this.createListRequest<HrmJobListFilter>(filter);
            listRequest.page.take = 999999;
            this.readList(listRequest).subscribe((listResult) => {
                let jobSeq = 1;
                if (listResult.total > 0) {
                    const seqList = listResult.data.map((hrmJob) => hrmJob.jobSeq);
                    jobSeq += Math.max(...seqList);
                }
                obs.next(jobSeq);
            });
        });
    }

    public get entityLookupList(): Observable<ComboBoxItemStringString[]> {
        return new Observable<ComboBoxItemStringString[]>((obs) => {
            this.lookupNumber().subscribe((listResult) => {
                if (listResult.total) {
                    const items = ComboBoxItemListBuilder.build(listResult.data, ComboBoxItemStringString, 'value', 'display');
                    obs.next(items);
                } else {
                    obs.next([]);
                }
            });
        });
    }


}
