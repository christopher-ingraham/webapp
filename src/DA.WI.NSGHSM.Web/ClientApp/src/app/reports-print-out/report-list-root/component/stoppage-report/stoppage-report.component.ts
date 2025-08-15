import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { MediaObserver } from '@angular/flex-layout';
import { Subject, Subscription } from 'rxjs';

import { ListRequest, LogService, AuthService } from '@app/core';

import {
    EntitySelectionHelper,
    ReportListItem,
    StoppageReportApiService,
    StoppageReportListFilter,
    StoppageReportListItem,
    StoppageReportBase,
    StoppageReport,
    StoppageReportDetail,
    StoppageReportForInsert,
    StoppageReportForUpdate,
    ReportCreateRequest,
    AppStatusStoreService,
} from '@app/shared';

import {
    BaseReportComponent,
    ReportListFiltersFromRoot,
    StoppageReportSelectionHelper,
} from '../../model';

@Component({
    selector: 'app-stoppage-report',
    templateUrl: './stoppage-report.component.html',
    styleUrls: ['./stoppage-report.component.css']
})
export class StoppageReportComponent
    extends BaseReportComponent<
    StoppageReportBase,
    StoppageReport,
    StoppageReportDetail,
    StoppageReportForInsert,
    StoppageReportForUpdate,
    StoppageReportListItem,
    StoppageReportListFilter,
    StoppageReportApiService>
    implements OnInit {

    @Input() public filters: Subject<ReportListFiltersFromRoot>;
    @Input() public row: StoppageReportSelectionHelper;
    @Input() public printer: Subject<StoppageReportListItem>;

    protected filter: StoppageReportListFilter;
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
        private stoppageReportApiService: StoppageReportApiService,
    ) {
        super('StoppageReport', log, authService, appStatus, media);
    }

    ngOnInit() {
        this.superNgOnInit();
        this.subscribe(
            this.filterSubscription,
            this.printerSubscription,
        );
        this.refreshData(() => this.enablePrint());
    }

    enablePrint() {
        if (this.rowSelectionHelper) {
            this.rowSelectionHelper.entity = new StoppageReportListItem();
            this.rowSelectionHelper.broadcast();
        }
    }

    get service() {
        return this.stoppageReportApiService;
    }

    protected refresh(filters: ReportListFiltersFromRoot): void {
        this.filter = new StoppageReportListFilter();
        if (filters) {
            this.filter.searchEndDelayDateFrom = filters.stoppageDate.searchFrom;
            this.filter.searchEndDelayDateTo = filters.stoppageDate.searchTo;
            this.filter.searchShiftId = filters.shiftId.search;
            this.filter.searchShiftLabel = filters.shiftId.isEnabled ? filters.shiftLabel.value : '';
            this.dataRequest = this.stoppageReportApiService.createListRequest<StoppageReportListFilter>(this.filter);
        }
        this.refreshData(() => this.enablePrint());
    }

    protected initDataRequest(): ListRequest<StoppageReportListFilter> {
        return {
            filter: new StoppageReportListFilter(),
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] },
        };
    }

    private get printerSubscription(): Subscription {
        return this.printer.subscribe((itemToPrint) => {
            /*
            4 - STOPPAGE
                repParams = new object[7];
                repParams[0] = reportId;
                repParams[1] = sFromDate;
                repParams[2] = sToDate;
                repParams[3] = sShiftId;
                repParams[4] = sShiftLabel;
            */
            const req = new ReportCreateRequest({
                reportLanguage: this.reportLanguage,
                reportType: 4,
                reportParam1: this.filter && this.filter.searchEndDelayDateFrom ?
                new Date(this.filter.searchEndDelayDateFrom).toISOString() : '---',
            reportParam2: this.filter && this.filter.searchEndDelayDateTo ?
                new Date(this.filter.searchEndDelayDateTo).toISOString() : '---',
            reportParam3: this.filter && this.filter.searchShiftId ? this.filter.searchShiftId : '---',
            reportParam4: this.filter && this.filter.searchShiftLabel ? this.filter.searchShiftLabel : '---',
            });
            this.stoppageReportApiService.downloadReport(req);
        });
    }

}
