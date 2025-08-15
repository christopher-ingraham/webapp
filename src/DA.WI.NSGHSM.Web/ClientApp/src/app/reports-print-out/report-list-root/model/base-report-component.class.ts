import { OnDestroy } from '@angular/core';
import { MediaObserver } from '@angular/flex-layout';
import { map } from 'rxjs/operators';
import { Subject, Subscription } from 'rxjs';

import { ListRequest, AuthService, LogService } from '@app/core';
import {
    BaseEntityService,
    BaseEntityListComponent,
    EntitySelectionHelper,
    ReportListItem,
    ReportCreateRequestLanguage,
    AppStatusStoreService,
} from '@app/shared';
import { SelectableSettings } from '@app/widget';

import { ReportListFiltersFromRoot } from './report-list-filters-from-root.class';

export abstract class BaseReportComponent<
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
    extends BaseEntityListComponent<TEntityBase,
    TEntity,
    TEntityDetail,
    TEntityForInsert,
    TEntityForUpdate,
    TEntityListItem,
    TEntityListFilter,
    TEntityApiService>
    implements OnDestroy {

    public readonly selectableSettings: SelectableSettings = {
        checkboxOnly: false,
        enabled: true,
        mode: 'single',
    };

    constructor(
        entityName: string,
        log: LogService,
        authService: AuthService,
        private appStatus: AppStatusStoreService,
        public media: MediaObserver,
    ) {
        super(log, authService, 'REPORT', entityName);
        this.dataRequest = this.initDataRequest();
    }

    protected abstract get filterSubject(): Subject<ReportListFiltersFromRoot>;
    protected abstract get rowSelectionHelper(): EntitySelectionHelper<ReportListItem>;
    abstract get service(): TEntityApiService;
    protected abstract refresh(filters: ReportListFiltersFromRoot): void;
    protected abstract initDataRequest(): ListRequest<TEntityListFilter>;

    protected superNgOnInit() {
        this.setupSecurityRoleFlags();
        this.isMobile = this.media.asObservable().pipe(
            map(() => this.media.isActive('xs'))
        );
        this.dataRequest = this.initDataRequest();
    }

    ngOnDestroy() {
        this.unsubscribeAll();
    }

    public onRowClick(event: { dataItem: ReportListItem }) {
        const row = this.rowSelectionHelper;
        if (row) {
            row.entity = event.dataItem;
            row.broadcast();
        }
    }

    protected get filterSubscription(): Subscription {
        return this.filterSubject.subscribe(
            (filters) => this.refresh(filters)
        );
    }

    protected get reportLanguage(): ReportCreateRequestLanguage {
        return this.appStatus.isUomSI ? 'English' : 'Usa';
    }

}
