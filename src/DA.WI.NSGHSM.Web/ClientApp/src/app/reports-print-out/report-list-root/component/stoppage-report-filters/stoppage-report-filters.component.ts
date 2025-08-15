import { Component, OnInit, Input } from '@angular/core';
import { Subject } from 'rxjs';

import { BaseReportFiltersComponent, ReportListFiltersFromRoot } from '../../model';
import { AuxValueService, ComboBoxItemNumberString } from '@app/shared';

@Component({
    selector: 'app-stoppage-report-filters',
    templateUrl: './stoppage-report-filters.component.html',
    styleUrls: ['./stoppage-report-filters.component.css']
})
export class StoppageReportFiltersComponent
    extends BaseReportFiltersComponent
    implements OnInit {

    protected get filterSubject(): Subject<ReportListFiltersFromRoot> {
        return this.filters;
    }

    @Input() public filters: Subject<ReportListFiltersFromRoot>;

    constructor(
        auxValueService: AuxValueService,
    ) {
        super(auxValueService, ReportListFiltersFromRoot.newForStoppageReport());
    }

    ngOnInit() {
        this.subscribe(
            this.shiftListSubscription,
        );
    }
    onShiftSelectionChange($event: ComboBoxItemNumberString) {
        this.filter.shiftLabel.value = $event.label;
    }
}
