
// rep-hm-piece-Trend-list.component.ts
import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { map, finalize } from 'rxjs/operators';
import { MediaObserver } from '@angular/flex-layout';

import { ListRequest, AuthService, LogService } from '@app/core';
import {
    BaseEntityListComponent,
    RepHmPieceTrend,
    RepHmPieceTrendApiService,
    RepHmPieceTrendBase,
    RepHmPieceTrendDetail,
    RepHmPieceTrendForInsert,
    RepHmPieceTrendForUpdate,
    RepHmPieceTrendListFilter,
    RepHmPieceTrendListFilterFromRoot,
    RepHmPieceTrendListItem,
    RepHmPieceSelectionHelper,
    RepHmPiece,
    RepHmPieceTrendSelectionHelper,
} from '@app/shared';
import { StringFilterCellComponent, GridComponent, GridDataResult } from '@progress/kendo-angular-grid';
import { Data } from '@angular/router';


@Component({
    selector: 'app-rep-hm-piece-trend-list',
    styleUrls: ['./rep-hm-piece-trend-list.component.scss'],
    templateUrl: 'rep-hm-piece-trend-list.component.html',
})
export class RepHmPieceTrendListComponent
    extends BaseEntityListComponent<RepHmPieceTrendBase,
    RepHmPieceTrend,
    RepHmPieceTrendDetail,
    RepHmPieceTrendForInsert,
    RepHmPieceTrendForUpdate,
    RepHmPieceTrendListItem,
    RepHmPieceTrendListFilter,
    RepHmPieceTrendApiService>
    implements OnInit {

    @Input() coil: RepHmPieceSelectionHelper;
    @Input() trends: RepHmPieceTrendSelectionHelper;
    //@ViewChild ('signalsGrid', {static: false}) signalsGrid: GridComponent;

    public chartsArray: RepHmPieceTrend[] = [];
    public selectedKeys: number[];
    public pageSizeTrends: number = 300;

    constructor(
        log: LogService,
        authService: AuthService,
        private repHmPieceTrendApiService: RepHmPieceTrendApiService,
        public media: MediaObserver,
    ) {
        super(log, authService, 'EXAMPLE', 'RepHmPieceTrend');
        this.dataRequest = this.initDataRequest();
    }

    get service() {
        return this.repHmPieceTrendApiService;
    }

    ngOnInit(): void {
        this.setupSecurityRoleFlags();
        this.isMobile = this.media.asObservable().pipe(map(() => this.media.isActive('xs')));
        this.subscribe(
            this.coil.subject.subscribe((repHmPiece) => this.processSelectedCoil(repHmPiece)),
        );
    }

    onFilterChange(filter: RepHmPieceTrendListFilter) {
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
            if (rows[0].dataItem.signalId === element.signalId) {
                const index = this.chartsArray.indexOf(element);
                this.chartsArray.splice(index, 1);
            }
        });
        this.trends.subject.next(this.chartsArray);
    }
    private processSelectedRows(rows) {
        const selectedRowIndex = rows[0].index;
        const rowDataItem = this.data.data[selectedRowIndex];
        const filter = new RepHmPieceTrendListFilter();
        filter.chartData = '1';
        filter.outPieceNoEq = '' + rowDataItem.outPieceNo;
        filter.passNoEq = '' + rowDataItem.passNo;
        filter.sampleIdEq = rowDataItem.signalId;
        filter.signalTypeEq = '' + rowDataItem.signalType;

        const listRequest = this.repHmPieceTrendApiService.createListRequest<RepHmPieceTrendListFilter>(filter);

        return this.repHmPieceTrendApiService.readList(listRequest).pipe(
            finalize(() => this.isLoading = false)
        ).subscribe((result) => {
            if (this.chartsArray !== null) {
                if (this.chartsArray.length < 3) {
                    this.chartsArray.push(result.data[0]);
                    this.trends.subject.next(this.chartsArray);
                } else {
                    alert('You can only select maximum 3 rows');
                    this.selectedKeys.splice(-1, 1);
                }
            }
        });
    }

   /* private processSelectedRowsChangeCoil(data: GridDataResult) {
        const selectedRowIndex = rows[0].index;
        const rowDataItem = this.data.data[selectedRowIndex % this.pageSize];
        const filter = new RepHmPieceTrendListFilter();
        filter.chartData = '1';
        filter.outPieceNoEq = '' + rowDataItem.outPieceNo;
        filter.passNoEq = '' + rowDataItem.passNo;
        filter.sampleIdEq = rowDataItem.signalId;
        filter.signalTypeEq = '' + rowDataItem.signalType;

        const listRequest = this.repHmPieceTrendApiService.createListRequest<RepHmPieceTrendListFilter>(filter);

        return this.repHmPieceTrendApiService.readList(listRequest).pipe(
            finalize(() => this.isLoading = false)
        ).subscribe((result) => {
            if (this.chartsArray !== null) {
                if (this.chartsArray.length < 3) {
                    this.chartsArray.push(result.data[0]);
                    this.trends.subject.next(this.chartsArray);
                } else {
                    alert('You can only select maximum 3 rows');
                    this.selectedKeys.splice(-1, 1);
                }
            }
        });
    }*/

    private initDataRequest(): ListRequest<RepHmPieceTrendListFilter> {
        const filter = new RepHmPieceTrendListFilter();
        const listRequest = this.repHmPieceTrendApiService.createListRequest<RepHmPieceTrendListFilter>(filter);
        listRequest.page.take = this.pageSizeTrends;
        return listRequest;
    }

    private processSelectedCoil(repHmPiece: RepHmPiece) {
        const filters = new RepHmPieceTrendListFilter();

        if (repHmPiece) {
            const ffr = new RepHmPieceTrendListFilterFromRoot(repHmPiece);
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
