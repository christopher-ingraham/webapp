import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { map, finalize } from 'rxjs/operators';
import { MediaObserver } from '@angular/flex-layout';

import { ListRequest, AuthService, LogService } from '@app/core';
import {
    BaseEntityListComponent,

    RepHmPieceSelectionHelper,
    RepHmPiece,
} from '@app/shared';
import {
    RepHmMapsViewBase,
    RepHmMapsView,
    RepHmMapsViewDetail,
    RepHmMapsViewForInsert,
    RepHmMapsViewForUpdate,
    RepHmMapsViewListItem,
    RepHmMapsViewListFilter,
    RepHmMapsViewApiService,
    RepHmMapsViewSelectionHelper,
    RepHmMapsViewListFilterFromRoot
} from 'src/app/shared/service/rep-hm-maps-view-api';


@Component({
    selector: 'app-rep-hm-maps-view-list',
    styleUrls: ['./rep-hm-maps-view-list.component.scss'],
    templateUrl: 'rep-hm-maps-view-list.component.html',
})
export class RepHmMapsViewListComponent
    extends BaseEntityListComponent<RepHmMapsViewBase,
    RepHmMapsView,
    RepHmMapsViewDetail,
    RepHmMapsViewForInsert,
    RepHmMapsViewForUpdate,
    RepHmMapsViewListItem,
    RepHmMapsViewListFilter,
    RepHmMapsViewApiService>
    implements OnInit {

    @Input() coil: RepHmPieceSelectionHelper;
    @Input() trends: RepHmMapsViewSelectionHelper;

    public chartsArray: RepHmMapsView[] = [];
    public selectedKeys: number[];

    constructor(
        log: LogService,
        authService: AuthService,
        private repHmMapsViewApiService: RepHmMapsViewApiService,
        public media: MediaObserver,
    ) {
        super(log, authService, 'EXAMPLE', 'RepHmMapsView');
        this.dataRequest = this.initDataRequest();
    }

    get service() {
        return this.repHmMapsViewApiService;
    }

    ngOnInit(): void {
        this.setupSecurityRoleFlags();
        this.isMobile = this.media.asObservable().pipe(map(() => this.media.isActive('xs')));
        this.subscribe(
            this.coil.subject.subscribe((repHmPiece) => this.processSelectedCoil(repHmPiece)),
        );
    }

    onFilterChange(filter: RepHmMapsViewListFilter) {
        this.dataRequest.filter = filter;
        this.dataRequest.filter.outPieceNoEq = filter.outPieceNoEq;
        this.dataRequest.page.skip = 0; // reset paging
        this.selectedKeys = [];
        this.chartsArray = [];
        this.trends.subject.next(this.chartsArray);
        this.refreshData();
    }

    public onRowSelectionChanged($event) {
        // process deselected rows
        if ($event.deselectedRows.length > 0) {
            this.processDeselectedRows($event.deselectedRows);
        }
        // process selected rows
        if ($event.selectedRows.length > 0) {
            this.processSelectedRows($event.selectedRows);
        }
    }

    private processDeselectedRows(rows) {
        this.chartsArray.forEach((element) => {
            if ((rows[0].dataItem.signalId === element.signalId)
                || (rows[0].dataItem.signalId === 'FM_' + element.signalId)
                || (rows[0].dataItem.signalId === 'RM_' + element.signalId)
                || (rows[0].dataItem.signalId === 'DC_' + element.signalId)) {
                const index = this.chartsArray.indexOf(element);
                this.chartsArray.splice(index, 1);
            }
        });
        this.trends.subject.next(this.chartsArray);
    }
    private processSelectedRows(rows) {
        const selectedRowIndex = rows[0].index;
        const rowDataItem = this.data.data[selectedRowIndex % this.pageSize];
        const filter = new RepHmMapsViewListFilter();
        filter.chartData = '1';
        filter.outPieceNoEq = '' + rowDataItem.outPieceNo;
        filter.passNoEq = '' + rowDataItem.passNo;
        filter.sampleIdEq = rowDataItem.signalId;
        filter.signalTypeEq = '' + rowDataItem.signalType;

        const listRequest = this.repHmMapsViewApiService.createListRequest<RepHmMapsViewListFilter>(filter);
        listRequest.page.take = 1;

        return this.repHmMapsViewApiService.readList(listRequest).pipe(
            finalize(() => this.isLoading = false)
        ).subscribe((result) => {
            if (this.chartsArray !== null) {
                if (this.chartsArray.length < 3) {
                    this.chartsArray.push(result.data[0]);
                    this.trends.subject.next(this.chartsArray);
                } else {
                    alert("You can only select maximum 3 rows");
                    this.selectedKeys.splice(-1, 1);
                }
            }
        });
    }

    private initDataRequest(): ListRequest<RepHmMapsViewListFilter> {
        const filter = new RepHmMapsViewListFilter();
        const listRequest = this.repHmMapsViewApiService.createListRequest<RepHmMapsViewListFilter>(filter);
        listRequest.page.take = this.pageSize;
        return listRequest;
    }

    private processSelectedCoil(repHmPiece: RepHmPiece) {
        const filters = new RepHmMapsViewListFilter();

        if (repHmPiece) {
            const ffr = new RepHmMapsViewListFilterFromRoot(repHmPiece);
            filters.centerIdEq = ffr.centerIdEq.search;
            filters.chartData = ffr.chartData.search;
            filters.outPieceNoEq = ffr.outPieceNoEq.search;
            filters.sampleIdEq = ffr.sampleIdEq.search;
            filters.sampleIdNe = ffr.sampleIdNe.search;
            filters.passNoEq = ffr.passNoEq.search;
        }
        this.onFilterChange(filters);
        this.log.debug('filter changed' + JSON.stringify(filters));
    }
}
