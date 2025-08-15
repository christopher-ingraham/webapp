import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';

import { SubscriptionList, AuxValueService, EntitySelectionHelper } from '@app/shared';
import { ConfirmService } from '@app/widget';

import { ReportListFiltersFromRoot, ReportListItem } from './model';
import { LogService } from '@app/core';
import { TimeOrientedReportFiltersComponent } from './component/time-oriented-report-filters/time-oriented-report-filters.component';
import { CoilGeneralReportFiltersComponent } from './component/coil-general-report-filters/coil-general-report-filters.component';
import { CobbleReportFiltersComponent } from './component/cobble-report-filters/cobble-report-filters.component';
import { PracticeReportFiltersComponent } from './component/practice-report-filters/practice-report-filters.component';
import { ShiftReportFiltersComponent } from './component/shift-report-filters/shift-report-filters.component';
import { StoppageReportFiltersComponent } from './component/stoppage-report-filters/stoppage-report-filters.component';

@Component({
    selector: 'app-report-list-root',
    templateUrl: './report-list-root.component.html',
    styleUrls: ['./report-list-root.component.css']
})
export class ReportListRootComponent
    extends SubscriptionList
    implements OnInit, OnDestroy {

    public initialized = false;
    public readonly modelOptions = { standalone: true };

    // filters
    public rootFilters = new Subject<ReportListFiltersFromRoot>();
    public filter: ReportListFiltersFromRoot;
    @ViewChild(TimeOrientedReportFiltersComponent, { static: false })
    protected timeOrientedReportFilters: TimeOrientedReportFiltersComponent;

    @ViewChild(CoilGeneralReportFiltersComponent, { static: false })
    protected coilGeneralReportFilters: CoilGeneralReportFiltersComponent;

    @ViewChild(CobbleReportFiltersComponent, { static: false })
    protected cobbleReportFilters: CobbleReportFiltersComponent;

    @ViewChild(PracticeReportFiltersComponent, { static: false })
    protected practiceReportFilters: PracticeReportFiltersComponent;

    @ViewChild(ShiftReportFiltersComponent, { static: false })
    protected shiftReportFilters: ShiftReportFiltersComponent;

    @ViewChild(StoppageReportFiltersComponent, { static: false })
    protected stoppageReportFilters: StoppageReportFiltersComponent;


    public reportSelection: EntitySelectionHelper<ReportListItem>;
    public printReport = new Subject<ReportListItem>();

    // Action buttons
    public exportAllDisabled = true; // TODO

    // Selected report
    public reportList: ReportListItem[] = [
        new ReportListItem(0, 'Coil General'),
        new ReportListItem(1, 'Time Oriented'),
        new ReportListItem(2, 'Shift'),
        new ReportListItem(3, 'Cobble'),
        new ReportListItem(4, 'Stoppage'),
        new ReportListItem(5, 'Practice'),
    ];
    public get reportId(): string {
        return this._reportId;
    }
    public set reportId(value: string) {
        this._reportId = value;
        this.reportSelection.entity = undefined;
    }
    private _reportId = '0';

    constructor(
        private log: LogService,
        private router: Router,
        private confirmService: ConfirmService,
        private auxValueService: AuxValueService,
    ) {
        super();
        this.log.verbose = true;
        this.reportSelection = new EntitySelectionHelper<ReportListItem>();
        this.subscribe(
            this.reportSelection.subscribe(),
            this.rootFilters.subscribe((f) => this.filter = f),
        );
    }

    ngOnInit() {
        this.initialized = true;
    }

    ngOnDestroy() {
        this.unsubscribeAll();
        this.log.verbose = false;
    }

    public applyFilters(event: KeyboardEvent) {
        this.rootFilters.next(this.filter);
    }

    public reportPrint(event: KeyboardEvent) {
        this.printReport.next(this.reportSelection.entity);
    }
    public reportRefresh(event: KeyboardEvent) {
        switch (this.reportId) {
            case '0':
            this.coilGeneralReportFilters.applyFilters(null);
            break;
            case '1':
            this.timeOrientedReportFilters.applyFilters(null);
            break;
            case '2':
            this.shiftReportFilters.applyFilters(null);
            break;
            case '3':
            this.cobbleReportFilters.applyFilters(null);
            break;
            case '4':
            this.stoppageReportFilters.applyFilters(null);
            break;
            case '5':
            this.practiceReportFilters.applyFilters(null);
            break;
        }
    }
    public reportExportAll(event: KeyboardEvent) {
        this.alertNotImplemented('reportExportAll');
    }

    private alertNotImplemented(what: string) {
        alert(`${ReportListRootComponent['name']}: "${what}" NOT implemented!`);
    }

}
