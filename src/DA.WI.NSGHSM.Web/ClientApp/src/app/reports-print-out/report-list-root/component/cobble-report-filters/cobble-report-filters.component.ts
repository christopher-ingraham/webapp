import { Component, OnInit, Input } from '@angular/core';
import { Subject } from 'rxjs';

import { BaseReportFiltersComponent, ReportListFiltersFromRoot } from '../../model';
import { AuxValueService } from '@app/shared';

@Component({
    selector: 'app-cobble-report-filters',
    templateUrl: './cobble-report-filters.component.html',
    styleUrls: ['./cobble-report-filters.component.css']
})
export class CobbleReportFiltersComponent
    extends BaseReportFiltersComponent
    implements OnInit {

    protected get filterSubject(): Subject<ReportListFiltersFromRoot> {
        return this.filters;
    }

    @Input() public filters: Subject<ReportListFiltersFromRoot>;

    constructor(
        auxValueService: AuxValueService,
    ) {
        super(auxValueService, ReportListFiltersFromRoot.newForCobbleReport());
    }

    ngOnInit() {
        this.subscribe(
            this.materialGradeIdListSubscription,
            this.coilStatusListSubscription,
            this.shiftListSubscription,
        );
    }

}
