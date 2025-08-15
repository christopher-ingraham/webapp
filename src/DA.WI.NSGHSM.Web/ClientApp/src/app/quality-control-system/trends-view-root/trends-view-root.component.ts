import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';

import { LogService, ListRequest, AuthService } from '@app/core';
import {
    BaseEntityListComponent,
    RepHmPiece,
    RepHmPieceListFilterFromRoot,
    RepHmPieceSelectionHelper,
    RepHmPieceTrend,
    RepHmPieceListFilter,
} from '@app/shared';

import {
    RepHmTrendsViewSelectionHelper,
    RepHmTrendsViewListFilter,
    RepHmTrendsViewApiService,
    RepHmTrendsView,
    RepHmTrendsViewListFilterFromRoot,
    RepHmTrendsViewBase,
    RepHmTrendsViewDetail,
    RepHmTrendsViewForInsert,
    RepHmTrendsViewForUpdate,
    RepHmTrendsViewListItem
} from 'src/app/shared/service/rep-hm-trends-view-api';
import { MediaObserver } from '@angular/flex-layout';
import { TrendsViewRootFiltersComponent } from './trends-view-root-filters/trends-view-root-filters.component';


@Component({
    selector: 'app-trends-view-root',
    templateUrl: './trends-view-root.component.html',
    styleUrls: ['./trends-view-root.component.css']
})
export class TrendsViewRootComponent
    extends BaseEntityListComponent<RepHmTrendsViewBase,
    RepHmTrendsView,
    RepHmTrendsViewDetail,
    RepHmTrendsViewForInsert,
    RepHmTrendsViewForUpdate,
    RepHmTrendsViewListItem,
    RepHmTrendsViewListFilter,
    RepHmTrendsViewApiService>
    implements OnInit, OnDestroy {

    @ViewChild(TrendsViewRootFiltersComponent, { static: false }) protected trendsViewRootFilters: TrendsViewRootFiltersComponent;

    public filters: Subject<RepHmPieceListFilterFromRoot>;
    public repHmPieceSelection: RepHmPieceSelectionHelper;
    public repHmTrendsViewSelection: RepHmTrendsViewSelectionHelper;
    public leftPaneCollapsed: Subject<boolean>;

    constructor(
        log: LogService,
        authService: AuthService,
        private repHmTrendsViewApiService: RepHmTrendsViewApiService,
        public media: MediaObserver,
    ) {
        super(log, authService, 'EXAMPLE', 'RepHmTrendsView');
        this.filters = new Subject<RepHmPieceListFilterFromRoot>();
        this.repHmTrendsViewSelection = new RepHmTrendsViewSelectionHelper();
        this.repHmPieceSelection = new RepHmPieceSelectionHelper();

        this.subscribe(
            this.repHmPieceSelection.subscribe(),
            this.repHmTrendsViewSelection.subscribe()
        );
        this.leftPaneCollapsed = new Subject<boolean>();
        this.dataRequest = this.initDataRequest();
    }

    get service() {
        return this.repHmTrendsViewApiService;
    }

    ngOnInit() {
        this.setupSecurityRoleFlags();
        this.subscribe(
            this.repHmPieceSelection.subject.subscribe((repHmPiece) => {
                this.processCoilForChartStackComponent(repHmPiece);
            }),
        );
    }

    ngOnDestroy() {
        this.unsubscribeAll();
    }

    public repHmPieceListRefresh(event: KeyboardEvent) {
        //this.filters.next(this.filter);
        this.trendsViewRootFilters.applyFilters(null);
    }

    public onRowSelection($event) {
        // this.processSelectedRows($event);
    }

    private processCoilForChartStackComponent(repHmPiece: RepHmPiece) {
        if (repHmPiece) {
            const filter = new RepHmTrendsViewListFilter();
            filter.outPieceNoEq = repHmPiece.outPieceNo.toString();
            const listRequest = this.repHmTrendsViewApiService.createListRequest<RepHmTrendsViewListFilter>(filter);
            this.repHmTrendsViewApiService.readList(listRequest).subscribe((listResult) => {
                const chartsArray: RepHmPieceTrend[] = listResult.data.map((item) => (item as RepHmPieceTrend));
                this.repHmTrendsViewSelection.subject.next(chartsArray);
            });
        }
    }

    onFilterChange(filter: RepHmTrendsViewListFilter) {
        this.dataRequest.filter = filter;
        this.dataRequest.filter.outPieceNoEq = filter.outPieceNoEq;
        this.dataRequest.page.skip = 0; // reset paging
        this.refreshData();
    }

    private initDataRequest(): ListRequest<RepHmTrendsViewListFilter> {
        const filter = new RepHmTrendsViewListFilter();
        const listRequest = this.repHmTrendsViewApiService.createListRequest<RepHmTrendsViewListFilter>(filter);
        listRequest.page.take = this.pageSize;
        return listRequest;
    }
}
