import { OnDestroy } from '@angular/core';
import { Subject, Subscription, Observable } from 'rxjs';

import { SubscriptionList, AuxValueService, ComboBoxItemNumberString } from '@app/shared';

import { ReportListFiltersFromRoot } from './report-list-filters-from-root.class';

export abstract class BaseReportFiltersComponent
    extends SubscriptionList
    implements OnDestroy {

    public readonly modelOptions = { standalone: true };
    public readonly numberFormatOptions = { maximumFractionDigits: 0 };

    public materialGradeIdList: ComboBoxItemNumberString[] = [];
    public coilStatusList: ComboBoxItemNumberString[] = [];
    public shiftList: ComboBoxItemNumberString[] = [];
    public millModeList: ComboBoxItemNumberString[] = [];

    constructor(
        protected auxValueService: AuxValueService,
        public filter: ReportListFiltersFromRoot,
    ) {
        super();
    }

    protected abstract get filterSubject(): Subject<ReportListFiltersFromRoot>;

    ngOnDestroy() {
        this.unsubscribeAll();
    }

    public applyFilters(event: KeyboardEvent) {
        this.filterSubject.next(this.filter);
    }

    protected get materialGradeIdListSubscription(): Subscription {
        return this.auxValueService.materialGradeList.subscribe(
            (list) => this.materialGradeIdList = list
        );
    }
    protected get coilStatusListSubscription(): Subscription {
        return this.auxValueService.producedCoilStateList.subscribe(
            (list) => this.coilStatusList = list
        );
    }
    protected get shiftListSubscription(): Subscription {
        return this.auxValueService.shiftIdList.subscribe(
            (list) => this.shiftList = list
        );
    }
    protected get millModeListSubscription(): Subscription {
        // TODO
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            const list = [
                new ComboBoxItemNumberString(0, 'FM ONLY (todo)'),
                new ComboBoxItemNumberString(1, 'RM + FM (todo)')
            ];
            obs.next(list);
        }).subscribe(
            (list) => this.millModeList = list
        );
    }
}
