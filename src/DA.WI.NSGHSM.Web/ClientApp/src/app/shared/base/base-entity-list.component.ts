import { Observable } from 'rxjs';
import { map, finalize } from 'rxjs/operators';

import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';

import { ListRequest, PagingRequest, AuthService, ListResult, LogService } from '@app/core';

import { BaseEntityService } from './base-entity.service';
import { SubscriptionList } from '../utility';
import { EventEmitter } from '@angular/core';


export abstract class BaseEntityListComponent<
    TEntityBase,
    TEntity,
    TEntityDetail,
    TEntityForInsert,
    TEntityForUpdate,
    TEntityListItem,
    TEntityListFilter,
    TEntityApiService extends BaseEntityService<
        TEntityBase,
        TEntity,
        TEntityDetail,
        TEntityForInsert,
        TEntityForUpdate,
        TEntityListItem,
        TEntityListFilter>>
    extends SubscriptionList {

    private readonly i18nPrefix: string;
    public readonly title: string;
    public readonly newEntityButtonLabel: string;
    public readonly filterPanelTitle: string;

    public isLoading = false;

    public isMobile: Observable<boolean>;

    // Role flags: All, Restricted, ReadOnly
    public isInAllRole = false;
    public isInRestrictedRole = false;
    public isReadOnly = false;

    public pageSize = 10;
    public selectedKeys = [];

    protected data: GridDataResult;
    protected dataRequest: ListRequest<TEntityListFilter>;

    constructor(
        protected log: LogService,
        private authService: AuthService,
        i18nPrefix = 'EXAMPLE',
        private readonly entityName = 'Entity'
    ) {
        super();
        this.i18nPrefix = `${i18nPrefix}.${entityName.toUpperCase()}`;
        this.title = `${this.i18nPrefix}.PAGETITLE.LIST|${this.entityName}`;
        this.newEntityButtonLabel = `${this.i18nPrefix}.NEW|New ${this.entityName}`;
        this.filterPanelTitle = `${this.i18nPrefix}.FILTER|Filter`;
    }

    abstract get service(): TEntityApiService;

    protected refreshData(callback?: () => void): void {
        this.fetchData().subscribe(
            (result) => {
                this.data = result;
                if (callback && (typeof callback === 'function')) {
                    callback();
                }
            }
        );
    }

    protected clearData() {
        this.data =  { total: 0, data: [] };
    }

    private fetchData(): Observable<GridDataResult> {
        this.isLoading = true;
        return this.service.readList(this.dataRequest).pipe(
            map((result: ListResult<TEntityListItem>) => ({
                data: result.data,
                total: result.total
            }) as GridDataResult),
            finalize(() => this.isLoading = false)
        );
    }

    onPageChange(event: PageChangeEvent): void {
        this.dataRequest.page = event as PagingRequest;
        this.refreshData();
    }

    protected setupSecurityRoleFlags() {
        const authService = this.authService;
        this.subscribe(
            authService.isInAllRole.subscribe((isInRole) => this.isInAllRole = isInRole),
            authService.isInRestrictedRole.subscribe((isInRole) => this.isInRestrictedRole = isInRole),
            authService.isInReadOnlyRole.subscribe((isInRole) => this.isReadOnly = isInRole),
        );
    }

}
