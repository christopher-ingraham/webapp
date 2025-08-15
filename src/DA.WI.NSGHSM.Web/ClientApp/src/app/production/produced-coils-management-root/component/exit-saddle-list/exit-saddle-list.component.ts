import { Component, OnInit, Output, EventEmitter, Input, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs';
import 'rxjs/add/observable/interval';

import { ListRequest, LogService, AuthService } from '@app/core';
import {
    AuxValueService,
    BaseEntityListComponent,
    ExitSaddle,
    ExitSaddleApiService,
    ExitSaddleBase,
    ExitSaddleDetail,
    ExitSaddleForInsert,
    ExitSaddleForUpdate,
    ExitSaddleListFilter,
    ExitSaddleListItem,
    // ExitSaddleSelectionHelper,
} from '@app/shared';

@Component({
    selector: 'app-exit-saddle-list',
    templateUrl: './exit-saddle-list.component.html',
    styleUrls: ['./exit-saddle-list.component.css']
})
export class ExitSaddleListComponent
    extends BaseEntityListComponent<
    ExitSaddleBase,
    ExitSaddle,
    ExitSaddleDetail,
    ExitSaddleForInsert,
    ExitSaddleForUpdate,
    ExitSaddleListItem,
    ExitSaddleListFilter,
    ExitSaddleApiService>
    implements OnInit, OnDestroy {


    constructor(
        log: LogService,
        authService: AuthService,
        private exitSaddleApiService: ExitSaddleApiService,
        private auxValueService: AuxValueService,
    ) {
        super(log, authService, 'EXAMPLE', 'ExitSaddle');
        this.dataRequest = this.initDataRequest();
    }

    get service() {
        return this.exitSaddleApiService;
    }

    ngOnInit() {
        this.setupSecurityRoleFlags();
        this.subscribe(
            this.auxValueService.producedCoilManagementExistSaddleListRefreshTimeout.subscribe((timeout) => {
                // sanitize and convert timeout for Observable.interval()
                const timeoutms = 1000 * (timeout || 1);
                this.subscribe(Observable.interval(timeoutms).subscribe((callNo) => this.refresh(callNo)));
            }),
        );
    }

    ngOnDestroy() {
        this.unsubscribeAll();
    }

    private initDataRequest(): ListRequest<ExitSaddleListFilter> {
        return {
            filter: new ExitSaddleListFilter(),
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] }
        };
    }

    private refresh(callNo: number) {
        this.refreshData();
    }
}
