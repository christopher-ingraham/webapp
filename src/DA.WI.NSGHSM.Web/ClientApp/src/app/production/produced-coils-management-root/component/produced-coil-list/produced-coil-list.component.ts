import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';

import { LogService, ListRequest, AuthService } from '@app/core';
import {
    BaseEntityListComponent,
    ProducedCoil,
    ProducedCoilApiService,
    ProducedCoilBase,
    ProducedCoilDetail,
    ProducedCoilForInsert,
    ProducedCoilForUpdate,
    ProducedCoilListFilter,
    ProducedCoilListItem,
    ProducedCoilSelectionHelper,
} from '@app/shared';
import { SelectableSettings } from '@app/widget';

import { ProducedCoilListFilterFromPcmRoot, ProducedCoilsManagementChildState } from '../../model';

@Component({
    selector: 'app-produced-coil-list',
    templateUrl: './produced-coil-list.component.html',
    styleUrls: ['./produced-coil-list.component.css']
})
export class ProducedCoilListComponent
    extends BaseEntityListComponent<
    ProducedCoilBase,
    ProducedCoil,
    ProducedCoilDetail,
    ProducedCoilForInsert,
    ProducedCoilForUpdate,
    ProducedCoilListItem,
    ProducedCoilListFilter,
    ProducedCoilApiService>
    implements OnInit, OnDestroy {

    get service(): ProducedCoilApiService {
        return this.producedCoilApiService;
    }

    @Input() public filters: Subject<ProducedCoilListFilterFromPcmRoot>;
    @Input() public row: ProducedCoilSelectionHelper;
    @Output() public edit = new EventEmitter();

    public stateForChild: ProducedCoilsManagementChildState = {
        // TO DO
    };

    public readonly selectableSettings: SelectableSettings = {
        checkboxOnly: false,
        enabled: true,
        mode: 'single',
    };

    constructor(
        log: LogService,
        authService: AuthService,
        private producedCoilApiService: ProducedCoilApiService,
    ) {
        super(log, authService, 'EXAMPLE', 'ProducedCoil');
        this.dataRequest = this.initDataRequest();
    }

    ngOnInit() {
        this.setupSecurityRoleFlags();
        this.subscribe(
            this.filters.subscribe((filters) => {
                const f = new ProducedCoilListFilter();
                f.searchProductionStopDateFrom = filters.productionStopDate.searchFrom;
                f.searchProductionStopDateTo = filters.productionStopDate.searchTo;
                f.searchProducedPieceId = filters.producedPieceId.search;
                f.searchInputSlabNumber = filters.inputSlabNumber.search;
                f.searchHeatNumber = filters.heatNumber.search;
                f.searchCoilStatus = filters.coilStatus.search;
                this.dataRequest = this.producedCoilApiService.createListRequest(f);
                this.refreshData();
            })
        );
        this.refreshData();
    }

    ngOnDestroy(): void {
        this.unsubscribeAll();
    }

    private initDataRequest(): ListRequest<ProducedCoilListFilter> {
        return {
            filter: new ProducedCoilListFilter(),
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] },
        };
    }

    public onRowClick(event: { dataItem: ProducedCoil }) {
        this.row.entity = event.dataItem;
        this.row.broadcast();
    }
    public onRowEdit(row: ProducedCoil) {
        this.edit.next(row);
    }

}
