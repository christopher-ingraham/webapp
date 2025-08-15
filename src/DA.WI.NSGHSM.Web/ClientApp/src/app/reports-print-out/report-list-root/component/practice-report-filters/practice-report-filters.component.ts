import { Component, OnInit, Input } from '@angular/core';
import { Subject } from 'rxjs';

import { BaseReportFiltersComponent, ReportListFiltersFromRoot } from '../../model';
import { AuxValueService } from '@app/shared';

@Component({
    selector: 'app-practice-report-filters',
    templateUrl: './practice-report-filters.component.html',
    styleUrls: ['./practice-report-filters.component.css']
})
export class PracticeReportFiltersComponent
    extends BaseReportFiltersComponent
    implements OnInit {

    protected get filterSubject(): Subject<ReportListFiltersFromRoot> {
        return this.filters;
    }

    @Input() public filters: Subject<ReportListFiltersFromRoot>;

    constructor(
        auxValueService: AuxValueService,
    ) {
        super(auxValueService, ReportListFiltersFromRoot.newForPracticeReport());
    }

    ngOnInit() {
        this.subscribe(
            this.millModeListSubscription,
            this.materialGradeIdListSubscription,
        );
    }

}
