import { Component, OnInit, Input } from '@angular/core';
import { Subject } from 'rxjs';

import { BaseReportFiltersComponent, ReportListFiltersFromRoot } from '../../model';
import { ComboBoxItemNumberString, AuxValueService } from '@app/shared';

@Component({
    selector: 'app-time-oriented-report-filters',
    templateUrl: './time-oriented-report-filters.component.html',
    styleUrls: ['./time-oriented-report-filters.component.css']
})
export class TimeOrientedReportFiltersComponent
    extends BaseReportFiltersComponent
    implements OnInit {

    protected get filterSubject(): Subject<ReportListFiltersFromRoot> {
        return this.filters;
    }

    @Input() public filters: Subject<ReportListFiltersFromRoot>;

    constructor(
        auxValueService: AuxValueService,
    ) {
        super(auxValueService, ReportListFiltersFromRoot.newForTimeOrientedReport());
    }

    onShiftSelectionChange($event: ComboBoxItemNumberString) {
        this.filter.shiftLabel.value = $event.label;
    }

    ngOnInit() {
        this.subscribe(
            this.shiftListSubscription,
        );
    }

}
