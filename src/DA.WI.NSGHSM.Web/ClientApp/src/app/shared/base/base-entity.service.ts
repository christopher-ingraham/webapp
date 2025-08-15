import { isDevMode } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map, finalize } from 'rxjs/operators';

import { classToPlain, plainToClass } from 'class-transformer';
import { saveAs, FileSaverOptions } from 'file-saver';

import {
    ApiService,
    AppError,
    AppErrorType,
    BadRequestError,
    DownloadResponse,
    ListRequest,
    ListResult,
    LogService,
    NotifierService,
    UrlHelperService,
} from '@app/core';

import { EntityLookup } from '../model/entity-lookup.interface';
import { EntityValidationMetadata } from '../model/entity-validation-metadata.class';
import { ReportCreateRequest } from '../model/report-create-request.class';
import { CasedObject } from '../utility';

export abstract class BaseEntityService<
    TEntityBase,
    TEntity,
    TEntityDetail,
    TEntityForInsert,
    TEntityForUpdate,
    TEntityListItem,
    TEntityListFilter> {

    // public static nameId = 'id';
    public static nameNewId = 'new';
    // public static nameCopyFrom = 'copyFrom';

    constructor(
        protected readonly log: LogService,
        protected readonly notifier: NotifierService,
        protected readonly apiService: ApiService,
        protected readonly urlHelperService: UrlHelperService,
        private readonly baseEntityUrl: string,
        private readonly baseEntityListUrl: string = null,
        private readonly baseEntityLookupUrl: string = null,
        private readonly baseEntityValidationUrl: string = null,
    ) {
        // Default behaviour: baseEntityListUrl is the same as
        // baseEntityUrl, if no special baseEntityListUrl present.
        if (!baseEntityListUrl) {
            this.baseEntityListUrl = baseEntityUrl;
        }
        if (!baseEntityLookupUrl) {
            this.baseEntityLookupUrl = baseEntityUrl;
        }
        if (!baseEntityValidationUrl) {
            this.baseEntityValidationUrl = baseEntityUrl;
        }
    }

    abstract dtoToClass(data: any): TEntityDetail;
    abstract dtoListToClassList(data: any[]): TEntityListItem[];

    private buildEntityUrl(relativeUrl: string = ''): string {
        return this.urlHelperService.combine(this.baseEntityUrl, relativeUrl);
    }

    private buildEntityListUrl(relativeUrl: string = ''): string {
        return this.urlHelperService.combine(this.baseEntityListUrl, relativeUrl);
    }

    private buildEntityLookupUrl(relativeUrl: string = ''): string {
        return this.urlHelperService.combine(this.baseEntityLookupUrl, relativeUrl);
    }

    private buildEntityValidationUrl(relativeUrl: string = ''): string {
        return this.urlHelperService.combine(this.baseEntityValidationUrl, relativeUrl);
    }

    public createListRequest<TListFilter>(filter: TListFilter): ListRequest<TListFilter> {
        return this.apiService.createListRequest<TListFilter>(filter);
    }

    public readList(listRequest: ListRequest<TEntityListFilter>): Observable<ListResult<TEntityListItem>> {
        return this.apiService.get<ListResult<TEntityListItem>>(this.buildEntityListUrl(), listRequest).pipe(
            map(x => ListResult.factory<TEntityListItem>(x.total, this.dtoListToClassList(x.data)))
        );
    }

    public read(id: number | string): Observable<TEntityDetail> {
        const entityId = (typeof id === 'number') ? id.toString() : id;
        return this.apiService.get(this.buildEntityUrl(entityId)).pipe(
            map(x => this.dtoToClass(x))
        );
    }

    public readFromListUrl(): Observable<TEntityDetail> {
        // Same as BaseEntityService.read, but from list URL and no ID.
        return this.apiService.get(this.buildEntityListUrl()).pipe(
            map(x => this.dtoToClass(x))
        );
    }

    public lookup<TId>(): Observable<ListResult<EntityLookup<TId>>> {
        return this.apiService.get<ListResult<EntityLookup<TId>>>(this.buildEntityLookupUrl()).pipe(
            map(x => ListResult.factory<EntityLookup<TId>>(x.total, x.data))
        );
    }
    public lookupNumber() {
        return this.lookup<number>();
    }
    public lookupString() {
        return this.lookup<string>();
    }

    public get validationMetadata(): Observable<EntityValidationMetadata<TEntityDetail>> {
        const entityUrl = this.buildEntityUrl();
        const entityValidationUrl = this.buildEntityValidationUrl();
        if (entityUrl === entityValidationUrl) {
            // Server won't provide contraints for Entity.
            // Pretend we got an empty constaint list.
            return new Observable<EntityValidationMetadata<TEntityDetail>>(
                (obs) => obs.next(plainToClass(EntityValidationMetadata, {}))
            );
        } else {
            return this.apiService.get<EntityValidationMetadata<TEntityDetail>>(entityValidationUrl).pipe(
                map((result) => {
                    const tmp: Partial<EntityValidationMetadata<TEntityDetail>> = CasedObject.camelCase(result, true);
                    return plainToClass(EntityValidationMetadata, tmp);
                })
            );
        }
    }

    public create(item: TEntityForInsert): Observable<TEntityDetail> {
        const url = this.buildEntityUrl(BaseEntityService.nameNewId);
        const payload = classToPlain(item);
        return this.apiService.post(url, payload).pipe(
            map((x) => {
                const y = this.dtoToClass(x);
                return y;
            }),
            catchError((e: AppError) =>
                throwError((e.type === AppErrorType.BAD_REQUEST) ? (e as BadRequestError).data : e)
            )
        );
    }

    public update(id: number, item: TEntityForUpdate): Observable<TEntityDetail> {
        const url = this.buildEntityUrl(id.toString());
        const payload = classToPlain(item);
        return this.apiService.put(url, payload).pipe(
            map(x => this.dtoToClass(x)),
            catchError((e: AppError) =>
                throwError((e.type === AppErrorType.BAD_REQUEST) ? (e as BadRequestError).data : e)
            )
        );
    }

    public delete(id: number): Observable<Object> {
        return this.apiService.delete(this.buildEntityUrl(id.toString())).pipe(
            map(x => x) // FIXME DTO?
        );
    }

    public downloadReport(reportCreateRequest: ReportCreateRequest) {
        const url = this.buildEntityUrl('report');
        this.apiService.download(url, reportCreateRequest).subscribe(
            (response: DownloadResponse) => {
                const options: FileSaverOptions = {
                    autoBom: false,
                };
                saveAs(response.data, response.name, options);
            }
        );
        if (isDevMode()) {
            // tslint:disable-next-line
            console.log(reportCreateRequest);
        }
    }
}
