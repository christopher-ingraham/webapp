import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';

import { LogService } from '@app/core';
import {
    RepHmPieceListFilterFromRoot,
    RepHmPieceSelectionHelper,
    SubscriptionList,
    RepHmPieceTrendSelectionHelper,
    ComboBoxItemNumberString,
} from '@app/shared';
import { MainSignalsRootFiltersComponent } from './main-signals-root-filters/main-signals-root-filters.component';

@Component({
    selector: 'app-main-signals-root',
    templateUrl: './main-signals-root.component.html',
    styleUrls: ['./main-signals-root.component.css']
})
export class MainSignalsRootComponent
    extends SubscriptionList
    implements OnInit, OnDestroy {

    @ViewChild(MainSignalsRootFiltersComponent, { static: false }) protected trendsViewRootFilters: MainSignalsRootFiltersComponent;
    public filters: Subject<RepHmPieceListFilterFromRoot>;
    public filter: RepHmPieceListFilterFromRoot;
    public repHmPieceSelection: RepHmPieceSelectionHelper;
    public repHmPieceTrendListSelection: RepHmPieceTrendSelectionHelper;

    public leftPaneCollapsed: Subject<boolean>;

    constructor(private log: LogService) {
        super();
        this.filters = new Subject<RepHmPieceListFilterFromRoot>();
        this.filter = new RepHmPieceListFilterFromRoot();
        this.repHmPieceTrendListSelection = new RepHmPieceTrendSelectionHelper();
        this.repHmPieceSelection = new RepHmPieceSelectionHelper();
        this.subscribe(
            this.repHmPieceSelection.subscribe(),
            this.repHmPieceTrendListSelection.subscribe(),
        );
        this.leftPaneCollapsed = new Subject<boolean>();
    }

    ngOnInit() {
    }
    ngOnDestroy() {
        this.unsubscribeAll();
    }

    public repHmPieceListRefresh(event: KeyboardEvent) {
        //this.filters.next(this.filter);
        this.trendsViewRootFilters.applyFilters(null);
    }

    public applyFilters(event: KeyboardEvent) {
        this.filters.next(this.filter);
    }
}
