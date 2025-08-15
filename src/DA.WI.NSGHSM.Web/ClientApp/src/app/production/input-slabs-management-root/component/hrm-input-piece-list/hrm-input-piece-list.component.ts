
// hrm-input-piece-list.component.ts
import { Component, OnInit, Input, OnDestroy, EventEmitter, Output, } from '@angular/core';
import { MediaObserver } from '@angular/flex-layout';
import { map } from 'rxjs/operators';
import { Subject } from 'rxjs';

import { ListRequest, AuthService, LogService } from '@app/core';
import {
    AuxValueService,
    BaseEntityListComponent,
    HrmInputPiece,
    HrmInputPieceApiService,
    HrmInputPieceBase,
    HrmInputPieceDetail,
    HrmInputPieceForInsert,
    HrmInputPieceForUpdate,
    HrmInputPieceListFilter,
    HrmInputPieceListItem,
    HrmInputPieceSelectionHelper,
    HrmJobSelectionHelper,
} from '@app/shared';
import { SelectableSettings } from '@app/widget';

import { HrmInputPieceListFilterFromIslmRoot, } from '../../model';
import { StringsListManagementChildState } from '../../../strings-list-management-root';


@Component({
    selector: 'app-hrm-input-piece-list',
    styleUrls: ['./hrm-input-piece-list.component.scss'],
    templateUrl: 'hrm-input-piece-list.component.html',
})
export class HrmInputPieceListComponent
    extends BaseEntityListComponent<
        HrmInputPieceBase,
        HrmInputPiece,
        HrmInputPieceDetail,
        HrmInputPieceForInsert,
        HrmInputPieceForUpdate,
        HrmInputPieceListItem,
        HrmInputPieceListFilter,
        HrmInputPieceApiService>
    implements OnInit, OnDestroy {

    @Input() public master?: HrmJobSelectionHelper;
    @Input() public row: HrmInputPieceSelectionHelper;
    @Output() public edit = new EventEmitter();

    @Input() public filters?: Subject<HrmInputPieceListFilterFromIslmRoot>;

    public stateForChild: StringsListManagementChildState = {
        pieceStatusList: [],
    };

    public readonly selectableSettings: SelectableSettings = {
        checkboxOnly: false,
        enabled: true,
        mode: 'single',
    };
    constructor(
        log: LogService,
        authService: AuthService,
        private hrmInputPieceApiService: HrmInputPieceApiService,
        private auxValueService: AuxValueService,
        public media: MediaObserver,
    ) {
        super(log, authService, 'EXAMPLE', 'HrmInputPiece');
        this.dataRequest = this.initDataRequest();
    }

    get service() {
        return this.hrmInputPieceApiService;
    }

    ngOnInit(): void {
        if (this.master) {
            this.subscribe(
                this.master.subject.subscribe((hrmJob) => {
                    if (hrmJob) {
                        this.dataRequest.filter.searchStringNo = hrmJob.jobId;
                        this.refreshData(() => this.afterRefreshData());
                    } else {
                        this.clearData();
                    }
                }),
                this.auxValueService.statusInputPieceList.subscribe((pieceStatusList) => {
                    this.stateForChild = { pieceStatusList, };
                }),
            );
        }
        if (this.filters) {
            this.subscribe(
                this.filters.subscribe((f) => {
                    this.dataRequest.filter.searchCreationTimeFrom = f.creationDateTime.searchFrom;
                    this.dataRequest.filter.searchCreationTimeTo = f.creationDateTime.searchTo;
                    this.dataRequest.filter.searchCustomerName = f.customerName.searchLike;
                    this.dataRequest.filter.searchCustomerOrderNo = f.customerOrderNumber.search;
                    this.dataRequest.filter.searchHeatNo = f.heatNumber.search;
                    this.dataRequest.filter.searchPieceStatus = f.pieceStatus.search;
                    this.dataRequest.filter.searchProductionStatusFrom = f.productionStatus.searchFrom;
                    this.dataRequest.filter.searchProductionStatusTo = f.productionStatus.searchTo;
                    this.dataRequest.filter.searchSlabNo = f.slabNumber.search;
                    this.dataRequest.filter.searchStatusLike = null; // FIXME Which filter is this from?
                    this.dataRequest.filter.searchStringNo = f.stringNumber.search;
                    this.refreshData(() => this.afterRefreshData());
                })
            );
        }
        this.setupSecurityRoleFlags();
        this.isMobile = this.media.asObservable().pipe(map(() => this.media.isActive('xs')));
        this.refreshData(() => this.afterRefreshData());
    }

    ngOnDestroy(): void {
        this.unsubscribeAll();
    }

    onFilterChange(filter: HrmInputPieceListFilter) {
        this.dataRequest.filter = filter;
        this.dataRequest.page.skip = 0; // reset paging
        this.refreshData(() => this.afterRefreshData());
    }

    private afterRefreshData() {
        // nothing to do
    }

    private initDataRequest(): ListRequest<HrmInputPieceListFilter> {
        return {
            filter: new HrmInputPieceListFilter(),
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] }
        };
    }

    public onRowClick(event: { dataItem: HrmInputPiece }) {
        this.row.entity = event.dataItem;
        this.row.broadcast();
    }

    public onRowEdit(row: HrmInputPiece) {
        this.edit.next(row);
    }

}
