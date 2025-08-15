
// hrm-job-list.component.ts
import { Component, OnInit, Output, EventEmitter, Input, OnDestroy } from '@angular/core';
import { map } from 'rxjs/operators';
import { MediaObserver } from '@angular/flex-layout';
import { Subject } from 'rxjs';

import { ListRequest, AuthService, LogService } from '@app/core';
import {
    AuxValueService,
    BaseEntityListComponent,
    HrmJob,
    HrmJobApiService,
    HrmJobBase,
    HrmJobDetail,
    HrmJobForInsert,
    HrmJobForUpdate,
    HrmJobListFilter,
    HrmJobListItem,
    HrmJobSelectionHelper,
} from '@app/shared';
import { SelectableSettings } from '@app/widget';
import { StringsListManagementChildState, HrmJobListFilterFromSlmRoot, } from '../../model';


@Component({
    selector: 'app-hrm-job-list',
    styleUrls: ['./hrm-job-list.component.scss'],
    templateUrl: 'hrm-job-list.component.html',
})
export class HrmJobListComponent
    extends BaseEntityListComponent<
    HrmJobBase,
    HrmJob,
    HrmJobDetail,
    HrmJobForInsert,
    HrmJobForUpdate,
    HrmJobListItem,
    HrmJobListFilter,
    HrmJobApiService>
    implements OnInit, OnDestroy {

    @Input() public filters: Subject<HrmJobListFilterFromSlmRoot>;
    @Input() public row: HrmJobSelectionHelper;
    @Output() public edit = new EventEmitter();

    public stateForChild: StringsListManagementChildState = {
        jobStatusList: [],
    };

    public readonly selectableSettings: SelectableSettings = {
        checkboxOnly: false,
        enabled: true,
        mode: 'single',
    };

    constructor(
        log: LogService,
        authService: AuthService,
        private hrmJobApiService: HrmJobApiService,
        private auxValueService: AuxValueService,
        public media: MediaObserver,
    ) {
        super(log, authService, 'EXAMPLE', 'HrmJob');
        this.dataRequest = this.initDataRequest();
    }

    get service() {
        return this.hrmJobApiService;
    }

    ngOnInit(): void {
        this.subscribe(
            this.filters.subscribe((filters) => {
                if (filters) {
                    // convert HrmJobListFilterFromRoot to HrmJobListFilter
                    const filter = new HrmJobListFilter({
                        searchDataFrom: filters.plannedProductionDate.searchFrom,
                        searchDataTo: filters.plannedProductionDate.searchTo,
                        searchJobId: filters.stringNumber.search,
                        searchProductionStatus: filters.productionStatus.search,
                    });
                    this.onFilterChange(filter);
                } else {
                    this.refreshData(() => this.afterRefreshData());
                }
            }),
            this.auxValueService.statusJobList.subscribe((list) => {
                this.stateForChild = {
                    isNew: false,
                    jobStatusList: list,
                };
            }),
        );
        this.refreshData(() => this.afterRefreshData());
        this.setupSecurityRoleFlags();
        this.isMobile = this.media.asObservable().pipe(map(() => this.media.isActive('xs')));
    }

    ngOnDestroy() {
        this.unsubscribeAll();
    }

    private afterRefreshData() {
        this.row.broadcastClear();
    }

    onFilterChange(filter: HrmJobListFilter) {
        this.dataRequest.filter = filter;
        this.dataRequest.page.skip = 0; // reset paging
        this.refreshData(() => this.afterRefreshData());
    }

    private initDataRequest(): ListRequest<HrmJobListFilter> {
        return {
            filter: new HrmJobListFilter(),
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] }
        };
    }

    public onRowClick(event: { dataItem: HrmJob }) {
        this.row.entity = event.dataItem;
        this.row.broadcast();
    }
    public onRowEdit(row: HrmJob) {
        this.edit.next(row);
    }
}
