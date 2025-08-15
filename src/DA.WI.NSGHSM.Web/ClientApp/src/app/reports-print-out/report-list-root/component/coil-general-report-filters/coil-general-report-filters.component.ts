import { Component, OnInit, Input } from '@angular/core';
import { Subject, Observable, Subscription } from 'rxjs';

import { ComboBoxItemNumberString, AuxValueService } from '@app/shared';

import { ReportListFiltersFromRoot, BaseReportFiltersComponent } from '../../model';

@Component({
    selector: 'app-coil-general-report-filters',
    templateUrl: './coil-general-report-filters.component.html',
    styleUrls: ['./coil-general-report-filters.component.css']
})
export class CoilGeneralReportFiltersComponent
    extends BaseReportFiltersComponent
    implements OnInit {

    protected get filterSubject(): Subject<ReportListFiltersFromRoot> {
        return this.filters;
    }

    @Input() public filters: Subject<ReportListFiltersFromRoot>;

    constructor(
        auxValueService: AuxValueService,
    ) {
        super(auxValueService, ReportListFiltersFromRoot.newForCoilGeneralReport());
    }

    ngOnInit() {
        this.subscribe(
            this.materialGradeIdListSubscription,
            this.coilStatusListSubscription,
            this.shiftListSubscription,
        );
    }

}
