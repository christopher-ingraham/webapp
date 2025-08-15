import { Component, OnInit, Input } from '@angular/core';
import { Subject } from 'rxjs';

import { BaseReportFiltersComponent, ReportListFiltersFromRoot } from '../../model';
import { AuxValueService } from '@app/shared';

@Component({
    selector: 'app-shift-report-filters',
    templateUrl: './shift-report-filters.component.html',
    styleUrls: ['./shift-report-filters.component.css']
})
export class ShiftReportFiltersComponent
    extends BaseReportFiltersComponent
    implements OnInit {

    protected get filterSubject(): Subject<ReportListFiltersFromRoot> {
        return this.filters;
    }

    @Input() public filters: Subject<ReportListFiltersFromRoot>;

    constructor(
        auxValueService: AuxValueService,
    ) {
        super(auxValueService, ReportListFiltersFromRoot.newForShiftReport());
    }

    ngOnInit() {
        this.subscribe(
            this.shiftListSubscription,
        );
    }

}
