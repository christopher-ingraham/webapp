
// rep-hm-piece-list.component.ts
import { Component, OnInit, Input, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { map } from 'rxjs/operators';
import { MediaObserver } from '@angular/flex-layout';

import { ListRequest, AuthService, LogService } from '@app/core';
import {
    BaseEntityListComponent,
    RepHmPiece,
    RepHmPieceApiService,
    RepHmPieceBase,
    RepHmPieceDetail,
    RepHmPieceForInsert,
    RepHmPieceForUpdate,
    RepHmPieceListFilter,
    RepHmPieceListFilterFromRoot,
    RepHmPieceListItem,
    RepHmPieceSelectionHelper,
} from '@app/shared';
import { SelectableSettings } from '@progress/kendo-angular-grid';
import { Subject } from 'rxjs';


@Component({
    selector: 'app-rep-hm-piece-list',
    styleUrls: ['./rep-hm-piece-list.component.scss'],
    templateUrl: 'rep-hm-piece-list.component.html',
})
export class RepHmPieceListComponent
    extends BaseEntityListComponent<
    RepHmPieceBase,
    RepHmPiece,
    RepHmPieceDetail,
    RepHmPieceForInsert,
    RepHmPieceForUpdate,
    RepHmPieceListItem,
    RepHmPieceListFilter,
    RepHmPieceApiService>
    implements OnInit {

    @Input() public filters: Subject<RepHmPieceListFilterFromRoot>;
    @Input() public row: RepHmPieceSelectionHelper;

    public readonly selectableSettings: SelectableSettings = {
        enabled: true,
        mode: 'single',
    };

    public selection: number[];

    constructor(
        log: LogService,
        authService: AuthService,
        private repHmPieceApiService: RepHmPieceApiService,
        public media: MediaObserver,
    ) {
        super(log, authService, 'EXAMPLE', 'RepHmPiece');
        this.dataRequest = this.initDataRequest();
    }

    get service() {
        return this.repHmPieceApiService;
    }

    ngOnInit(): void {
        this.setupSecurityRoleFlags();
        this.isMobile = this.media.asObservable().pipe(map(() => this.media.isActive('xs')));
        this.subscribe(
            this.filters.subscribe((ffr) => this.processFiltersFromRoot(ffr)),
        );
        this.refreshData(() => this.afterRefreshData());
    }

    onFilterChange(filter: RepHmPieceListFilter) {
        this.dataRequest.filter = filter;
        this.dataRequest.filter.searchProducedCoil = filter.searchProducedCoil ? filter.searchProducedCoil : null;
        this.dataRequest.page.skip = 0; // reset paging
        this.refreshData();
    }

    public onRowClick(event: { dataItem: RepHmPiece }) {
        this.row.entity = event.dataItem;
        //this.row.broadcast();
    }

    private afterRefreshData() {
        this.row.broadcastClear();
    }

    private initDataRequest(): ListRequest<RepHmPieceListFilter> {
        return {
            filter: new RepHmPieceListFilter(),
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] }
        };
    }


    private processFiltersFromRoot(ffr) {
        if (ffr) {
            // convert RepHmPieceListFilterFromRoot to RepHmPieceListFilter
            const filters = new RepHmPieceListFilter({
                searchDataFrom: ffr.productionStopDate.searchFrom,
                searchDataTo: ffr.productionStopDate.searchTo,
                searchInputCoil: ffr.inputCoilId.search,
                searchProducedCoil: ffr.producedCoilId.search,
            });
            this.onFilterChange(filters);
            this.log.debug('filters changed' + JSON.stringify(filters));
        } else {
            this.log.debug('filters DID NOT change');
            this.refreshData(() => this.afterRefreshData());
        }
    }
}
