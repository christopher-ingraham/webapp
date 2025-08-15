import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { MediaObserver } from '@angular/flex-layout';
import { Subject, Subscription } from 'rxjs';

import { ListRequest, LogService, AuthService } from '@app/core';

import {
    EntitySelectionHelper,
    PracticeReport,
    PracticeReportApiService,
    PracticeReportBase,
    PracticeReportDetail,
    PracticeReportForInsert,
    PracticeReportForUpdate,
    PracticeReportListFilter,
    PracticeReportListItem,
    ReportListItem,
    ReportCreateRequest,
    AppStatusStoreService,
} from '@app/shared';

import {
    BaseReportComponent,
    ReportListFiltersFromRoot,
    PracticeReportSelectionHelper,
} from '../../model';

@Component({
    selector: 'app-practice-report',
    templateUrl: './practice-report.component.html',
    styleUrls: ['./practice-report.component.css']
})
export class PracticeReportComponent
    extends BaseReportComponent<
    PracticeReportBase,
    PracticeReport,
    PracticeReportDetail,
    PracticeReportForInsert,
    PracticeReportForUpdate,
    PracticeReportListItem,
    PracticeReportListFilter,
    PracticeReportApiService>
    implements OnInit {

    @Input() public filters: Subject<ReportListFiltersFromRoot>;
    @Input() public row: PracticeReportSelectionHelper;
    @Input() public printer: Subject<PracticeReportListItem>;

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
        private practiceReportApiService: PracticeReportApiService,
    ) {
        super('PracticeReport', log, authService, appStatus, media);
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
        return this.practiceReportApiService;
    }

    protected refresh(filters: ReportListFiltersFromRoot): void {
        if (filters) {
            const filter = new PracticeReportListFilter();
            filter.searchCenterId = filters.centerId.search;
            filter.searchEntryThicknessFrom = filters.entryThickness.searchFrom;
            filter.searchEntryThicknessTo = filters.entryThickness.searchTo;
            filter.searchMaterialGradeId = filters.materialGradeId.search;
            filter.searchMillMode = filters.millMode.search;
            filter.searchPracticeId = filters.practiceId.search;
            filter.searchRevisionDateFrom = filters.revisionDate.searchFrom;
            filter.searchRevisionDateTo = filters.revisionDate.searchTo;
            this.dataRequest = this.practiceReportApiService.createListRequest<PracticeReportListFilter>(filter);
        }
        if (this.rowSelectionHelper) {
            this.rowSelectionHelper.entity = null;
            this.rowSelectionHelper.broadcast();
        }
        this.refreshData();
    }

    protected initDataRequest(): ListRequest<PracticeReportListFilter> {
        return {
            filter: new PracticeReportListFilter(),
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] },
        };
    }

    private get printerSubscription(): Subscription {
        return this.printer.subscribe((itemToPrint) => {
            /*
            5 - PRACTICE
                repParams = new object[3];
                repParams[1] = sPracticeId;
                repParams[2] = nDisabledStand;
                repParams[3] = nMillMode;
            */
            const req = new ReportCreateRequest({
                reportLanguage: this.reportLanguage,
                reportType: 5,
                reportParam1: itemToPrint.practiceId,
                reportParam2: itemToPrint.nDisabledStand.toString(),
                reportParam3: itemToPrint.millMode.toString(),
            });
            this.practiceReportApiService.downloadReport(req);
        });
    }

}
