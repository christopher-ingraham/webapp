import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { MediaObserver } from '@angular/flex-layout';
import { Subject, Subscription } from 'rxjs';

import { ListRequest, LogService, AuthService } from '@app/core';

import {
    EntitySelectionHelper,
    ReportListItem,
    ShiftReportApiService,
    ShiftReportListFilter,
    ShiftReportListItem,
    ShiftReportBase,
    ShiftReport,
    ShiftReportDetail,
    ShiftReportForInsert,
    ShiftReportForUpdate,
    ReportCreateRequest,
    AppStatusStoreService,
} from '@app/shared';

import {
    BaseReportComponent,
    ReportListFiltersFromRoot,
    ShiftReportSelectionHelper,
} from '../../model';

@Component({
    selector: 'app-shift-report',
    templateUrl: './shift-report.component.html',
    styleUrls: ['./shift-report.component.css']
})
export class ShiftReportComponent
    extends BaseReportComponent<
    ShiftReportBase,
    ShiftReport,
    ShiftReportDetail,
    ShiftReportForInsert,
    ShiftReportForUpdate,
    ShiftReportListItem,
    ShiftReportListFilter,
    ShiftReportApiService>
    implements OnInit {

    @Input() public filters: Subject<ReportListFiltersFromRoot>;
    @Input() public row: ShiftReportSelectionHelper;
    @Input() public printer: Subject<ShiftReportListItem>;

    protected get filterSubject(): Subject<ReportListFiltersFromRoot> {
        return this.filters;
    }
    protected get rowSelectionHelper(): EntitySelectionHelper<ReportListItem> {
        return this.row;
    }

    constructor(
        authService: AuthService,
        log: LogService,
        media: MediaObserver,
        appStatus: AppStatusStoreService,
        private shiftReportApiService: ShiftReportApiService,
    ) {
        super('ShiftReport', log, authService, appStatus, media);
    }

    ngOnInit() {
        this.superNgOnInit();
        this.subscribe(
            this.filterSubscription,
            this.printerSubscription,
        );
        this.refreshData();
    }

    get service() {
        return this.shiftReportApiService;
    }

    protected refresh(filters: ReportListFiltersFromRoot): void {
        if (filters) {
            const filter = new ShiftReportListFilter();
            filter.searchShiftId = filters.shiftId.search;
            filter.searchShiftStartDateFrom = filters.shiftDate.searchFrom;
            filter.searchShiftStartDateTo = filters.shiftDate.searchTo;
            this.dataRequest = this.shiftReportApiService.createListRequest<ShiftReportListFilter>(filter);
        }
        if (this.rowSelectionHelper) {
            this.rowSelectionHelper.entity = null;
            this.rowSelectionHelper.broadcast();
        }
        this.refreshData();
    }

    protected initDataRequest(): ListRequest<ShiftReportListFilter> {
        return {
            filter: new ShiftReportListFilter(),
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] },
        };
    }

    private get printerSubscription(): Subscription {
        return this.printer.subscribe((itemToPrint: ShiftReportListItem) => {
            /*
            2 - SHIFT
                repParams = new object[7];
                repParams[0] = reportId;
                repParams[1] = nShiftCnt;
                repParams[2] = sArrShiftId;
                repParams[3] = sArrFromDate;
                repParams[4] = sArrToDate;
                repParams[5] = nLastPieceNo;
                repParams[6] = 1; // parameters array size
            */
            const req = new ReportCreateRequest({
                reportLanguage: this.reportLanguage,
                reportType: 2,
                reportParam1: itemToPrint.shiftCnt.toString(),
                reportParam2: itemToPrint.shiftId.toString(),
                reportParam3: itemToPrint.shiftStartDate.toISOString(),
                reportParam4: itemToPrint.shiftEndDate.toISOString(),
                reportParam5: itemToPrint.lastPieceNo.toString(),
            });
            this.shiftReportApiService.downloadReport(req);
        });
    }

}
