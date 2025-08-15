import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';

import { LogService, ListRequest, AuthService } from '@app/core';
import {
    BaseEntityListComponent,
    ProducedCoilSelectionHelper,
    UsedSetup,
    UsedSetupApiService,
    UsedSetupBase,
    UsedSetupDetail,
    UsedSetupForInsert,
    UsedSetupForUpdate,
    UsedSetupListFilter,
    UsedSetupListItem,
    UsedSetupSelectionHelper,
} from '@app/shared';
import { SelectableSettings } from '@app/widget';

@Component({
    selector: 'app-used-setup-list',
    templateUrl: './used-setup-list.component.html',
    styleUrls: ['./used-setup-list.component.css']
})
export class UsedSetupListComponent
    extends BaseEntityListComponent<
    UsedSetupBase,
    UsedSetup,
    UsedSetupDetail,
    UsedSetupForInsert,
    UsedSetupForUpdate,
    UsedSetupListItem,
    UsedSetupListFilter,
    UsedSetupApiService>
    implements OnInit, OnDestroy {

    @Input() master: ProducedCoilSelectionHelper;
    @Input() row: UsedSetupSelectionHelper;
    @Output() public edit = new EventEmitter();

    get service(): UsedSetupApiService {
        return this.producedCoilApiService;
    }

    public readonly selectableSettings: SelectableSettings = {
        checkboxOnly: false,
        enabled: true,
        mode: 'single',
    };

    constructor(
        log: LogService,
        authService: AuthService,
        private producedCoilApiService: UsedSetupApiService,
    ) {
        super(log, authService, 'EXAMPLE', 'UsedSetup');
        this.dataRequest = this.initDataRequest();
    }

    ngOnInit() {
        this.setupSecurityRoleFlags();
        this.subscribe(
            this.master.subject.subscribe((producedCoil) => {
                if (this.master.isSelected) {
                    this.dataRequest.filter.searchInPieceNo = producedCoil.inPieceNo;
                    this.refreshData();
                } else {
                    this.clearData();
                }
            })
        );
    }

    ngOnDestroy(): void {
        this.unsubscribeAll();
    }

    private initDataRequest(): ListRequest<UsedSetupListFilter> {
        return {
            filter: new UsedSetupListFilter(),
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] },
        };
    }

    public onRowClick(event: { dataItem: UsedSetup }) {
        this.row.entity = event.dataItem;
        this.row.broadcast();
    }
    public onRowEdit(row: UsedSetup) {
        this.edit.next(row);
    }

}
