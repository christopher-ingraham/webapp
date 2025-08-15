import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { MediaObserver } from '@angular/flex-layout';
import { Subject, Subscription } from 'rxjs';

import { ListRequest, LogService, AuthService } from '@app/core';

import {
    CoilGeneralReportApiService,
    CoilGeneralReportListFilter,
    CoilGeneralReportListItem,
    CoilGeneralReportBase,
    CoilGeneralReport,
    CoilGeneralReportDetail,
    CoilGeneralReportForInsert,
    CoilGeneralReportForUpdate,
    EntitySelectionHelper,
    ReportListItem,
    ReportCreateRequest,
    AppStatusStoreService,
} from '@app/shared';

import {
    BaseReportComponent,
    CoilGeneralReportSelectionHelper,
    ReportListFiltersFromRoot,
} from '../../model';

@Component({
    selector: 'app-time-oriented-report',
    templateUrl: './time-oriented-report.component.html',
    styleUrls: ['./time-oriented-report.component.css']
})
export class TimeOrientedReportComponent
    extends BaseReportComponent<
    CoilGeneralReportBase,
    CoilGeneralReport,
    CoilGeneralReportDetail,
    CoilGeneralReportForInsert,
    CoilGeneralReportForUpdate,
    CoilGeneralReportListItem,
    CoilGeneralReportListFilter,
    CoilGeneralReportApiService>
    implements OnInit {

    @Input() public filters: Subject<ReportListFiltersFromRoot>;
    @Input() public row: CoilGeneralReportSelectionHelper;
    @Input() public printer: Subject<CoilGeneralReportListItem>;

    protected filter: CoilGeneralReportListFilter;    

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
        private coilGeneralReportApiService: CoilGeneralReportApiService
    ) {
        super('CoilGeneralReport', log, authService, appStatus, media);
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
            this.rowSelectionHelper.entity = new CoilGeneralReportListItem();
            this.rowSelectionHelper.broadcast();
        }
    }

    get service(): CoilGeneralReportApiService {
        return this.coilGeneralReportApiService;
    }

    protected refresh(filters: ReportListFiltersFromRoot): void {
        this.filter = new CoilGeneralReportListFilter();
        if (filters) {
            this.filter.searchProductionStopDateFrom = filters.productionStopDate.searchFrom;
            this.filter.searchProductionStopDateTo = filters.productionStopDate.searchTo;
            this.filter.searchShiftId = filters.shiftId.search;
            this.filter.searchShiftLabel = filters.shiftId.isEnabled ? filters.shiftLabel.value : '';
            this.dataRequest = this.coilGeneralReportApiService.createListRequest<CoilGeneralReportListFilter>(this.filter);
        }
        this.refreshData(() => this.enablePrint());
    }

    protected initDataRequest(): ListRequest<CoilGeneralReportListFilter> {
        return {
            filter: new CoilGeneralReportListFilter(),
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] },
        };
    }

    private get printerSubscription(): Subscription {
        return this.printer.subscribe((itemToPrint: CoilGeneralReportListItem) => {
            /*
            1 - TIME_PRODUCTION
                repParams = new object[7];
                repParams[0] = reportId;
                repParams[1] = sFromDate;
                repParams[2] = sToDate;
                repParams[3] = sShiftId;
                repParams[4] = sShiftLabel;
            */
            const req = new ReportCreateRequest({
                reportLanguage: this.reportLanguage,
                reportType: 1,
                reportParam1: this.filter && this.filter.searchProductionStopDateFrom ?
                    new Date(this.filter.searchProductionStopDateFrom).toISOString() : '---',
                reportParam2: this.filter && this.filter.searchProductionStopDateTo ?
                    new Date(this.filter.searchProductionStopDateTo).toISOString() : '---',
                reportParam3: this.filter && this.filter.searchShiftId ? this.filter.searchShiftId : '---',
                reportParam4: this.filter && this.filter.searchShiftLabel ? this.filter.searchShiftLabel : '---',
            });
            this.coilGeneralReportApiService.downloadReport(req);
        });
    }

}
