import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';

import { LogService } from '@app/core';
import {
    RepHmPieceListFilterFromRoot,
    RepHmPieceSelectionHelper,
    SubscriptionList
} from '@app/shared';
import { RepHmMapsViewSelectionHelper } from 'src/app/shared/service/rep-hm-maps-view-api';
import { filter } from 'rxjs/operators';
import { MapsViewRootFiltersComponent } from './maps-view-root-filters/maps-view-root-filters.component';


@Component({
    selector: 'app-maps-view-root',
    templateUrl: './maps-view-root.component.html',
    styleUrls: ['./maps-view-root.component.css']
})
export class MapsViewRootComponent
    extends SubscriptionList
    implements OnInit, OnDestroy {

    @ViewChild(MapsViewRootFiltersComponent, { static: false }) protected mapsViewRootFilters: MapsViewRootFiltersComponent;

    public filters: Subject<RepHmPieceListFilterFromRoot>;
    public filter: RepHmPieceListFilterFromRoot;
    public repHmPieceSelection: RepHmPieceSelectionHelper;
    public repHmMapsViewSelectionHelper: RepHmMapsViewSelectionHelper;

    public leftPaneCollapsed: Subject<boolean>;

    constructor(private log: LogService) {
        super();
        this.filters = new Subject<RepHmPieceListFilterFromRoot>();
        this.filter = new RepHmPieceListFilterFromRoot();
        this.repHmMapsViewSelectionHelper = new RepHmMapsViewSelectionHelper();
        this.repHmPieceSelection = new RepHmPieceSelectionHelper();
        this.subscribe(
            this.repHmPieceSelection.subscribe(),
            this.repHmMapsViewSelectionHelper.subscribe(),
        );
        this.leftPaneCollapsed = new Subject<boolean>();
    }

    ngOnInit() {
    }
    ngOnDestroy() {
        this.unsubscribeAll();
    }

    public repHmPieceListRefresh(event: KeyboardEvent) {
        this.mapsViewRootFilters.applyFilters(null);
    }
}
