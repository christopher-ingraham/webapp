import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { MediaObserver } from '@angular/flex-layout';
import { Subject, Subscription } from 'rxjs';

import { ListRequest, LogService, AuthService } from '@app/core';

import {
    CoilGeneralReport,
    CoilGeneralReportApiService,
    CoilGeneralReportBase,
    CoilGeneralReportDetail,
    CoilGeneralReportForInsert,
    CoilGeneralReportForUpdate,
    CoilGeneralReportListFilter,
    CoilGeneralReportListItem,
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
    selector: 'app-coil-general-report',
    templateUrl: './coil-general-report.component.html',
    styleUrls: ['./coil-general-report.component.css']
})
export class CoilGeneralReportComponent
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

    protected get filterSubject(): Subject<ReportListFiltersFromRoot> {
        return this.filters;
    }
    protected get rowSelectionHelper(): EntitySelectionHelper<ReportListItem> {
        return this.row as EntitySelectionHelper<ReportListItem>;
    }

    constructor(
        authService: AuthService,
        log: LogService,
        media: MediaObserver,
        appStatus: AppStatusStoreService,
        private coilGeneralReportApiService: CoilGeneralReportApiService
    ) {
        super('CoilGeneralReport', log, authService,appStatus,  media);
    }

    ngOnInit() {
        this.superNgOnInit();
        this.subscribe(
            this.filterSubscription,
            this.printerSubscription,
        );
        this.refreshData();
    }

    get service(): CoilGeneralReportApiService {
        return this.coilGeneralReportApiService;
    }

    protected refresh(filters: ReportListFiltersFromRoot): void {
        const filter = new CoilGeneralReportListFilter();
        if (filters) {
            filter.searchCoilStatus = filters.coilStatus.search;
            filter.searchExitThicknessFrom = filters.exitThickness.searchFrom;
            filter.searchExitThicknessTo = filters.exitThickness.searchTo;
            filter.searchExitWidthFrom = filters.exitWidth.searchFrom;
            filter.searchExitWidthTo = filters.exitWidth.searchTo;
            filter.searchHeatNumber = filters.heatNumber.search;
            filter.searchInputSlabNumber = filters.inputSlabNumber.search;
            filter.searchMaterialGradeId = filters.materialGradeId.search;
            filter.searchProducedPieceId = filters.producedPieceId.search;
            filter.searchProductionStopDateFrom = filters.productionStopDate.searchFrom;
            filter.searchProductionStopDateTo = filters.productionStopDate.searchTo;
            filter.searchShiftId = filters.shiftId.search;
            this.dataRequest = this.coilGeneralReportApiService.createListRequest<CoilGeneralReportListFilter>(filter);
        }
        if (this.rowSelectionHelper) {
            this.rowSelectionHelper.entity = null;
            this.rowSelectionHelper.broadcast();
        }

        this.refreshData();
    }

    protected initDataRequest(): ListRequest<CoilGeneralReportListFilter> {
        return {
            filter: new CoilGeneralReportListFilter(),
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] },
        };
    }

    private get printerSubscription(): Subscription {
        return this.printer.subscribe((itemToPrint) => {
            /*
            0 - COIL_PRODUCTION
                repParams = new object[4];
                repParams[0] = reportId;
                repParams[1] = nOutCoilNo;
                repParams[2] = sOutCoilId;
                repParams[3] = 1; // parameters array size
            */
            const req = new ReportCreateRequest({
                reportLanguage: this.reportLanguage,
                reportType: 0,
                reportParam1: itemToPrint.outPieceNo.toString(),
                reportParam2: itemToPrint.outPieceId,
            });
            this.coilGeneralReportApiService.downloadReport(req);
        });
    }
}
