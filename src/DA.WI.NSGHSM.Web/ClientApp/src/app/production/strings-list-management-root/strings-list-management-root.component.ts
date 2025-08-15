import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';

import {
    AuxValueService,
    BaseMasterDetailTabbedComponent,
    ComboBoxItemNumberString,
    HrmInputPieceSelectionHelper,
    HrmJobApiService,
    HrmJobSelectionHelper,
    HrmJobDetail,
    HrmInputPieceDetail,
    HrmJobListItem,
    HrmInputPiece,
} from '@app/shared';

import { HrmJobListFilterFromSlmRoot, } from './model';
import { ConfirmService } from '@app/widget';
import { AuthService } from '@app/core';

const HrmJobDetailName = 'HrmJobDetail';
const HrmInputPieceDetailName = 'HrmInputPieceDetail';

@Component({
    selector: 'app-strings-list-management-root',
    templateUrl: './strings-list-management-root.component.html',
    styleUrls: ['./strings-list-management-root.component.css']
})
export class StringsListManagementRootComponent
    extends BaseMasterDetailTabbedComponent
    implements OnInit, OnDestroy {

    // filters
    public filters: Subject<HrmJobListFilterFromSlmRoot>;
    public filter: HrmJobListFilterFromSlmRoot;
    public productionStatusList: ComboBoxItemNumberString[] = [];
    public set productionStatusValue(item: ComboBoxItemNumberString) {
        this.filter.productionStatus.value = item.value;
    }

    // HrmJob list
    public jobSelection: HrmJobSelectionHelper;

    // HrmInputPiece list
    public inputPieceSelection: HrmInputPieceSelectionHelper;

    constructor(
        private confirmService: ConfirmService,
        private auxValueService: AuxValueService,
        private hrmJobApiService: HrmJobApiService,
        authService: AuthService,
    ) {
        super(authService, 'Strings List');

        this.filters = new Subject<HrmJobListFilterFromSlmRoot>();
        this.filter = new HrmJobListFilterFromSlmRoot();

        this.jobSelection = new HrmJobSelectionHelper();
        this.inputPieceSelection = new HrmInputPieceSelectionHelper();
        this.subscribe(
            this.jobSelection.subscribe(),
            this.inputPieceSelection.subscribe(),
            this.auxValueService.statusJobList.subscribe((list) => {
                this.productionStatusList = list;
            }),
            this.tabstrip.tabClosed.subscribe((event) => {
                switch (event.entityName) {
                    case HrmJobDetailName:
                        this.filters.next();
                        break;
                    case HrmInputPieceDetailName:
                        this.jobSelection.broadcast();
                        break;
                    default:
                        // processed by superclass
                        break;
                }
            }),
        );
    }

    ngOnInit() {
        this.setupSecurityRoleFlags();
    }

    public applyFilters(event: KeyboardEvent) {
        this.filters.next(this.filter);
    }

    // String List toolbar button handlers
    public stringNew(event: KeyboardEvent) {
        const title = `New String`;
        this.tabstrip.append({
            title,
            type: HrmJobDetailName,
            isNew: true,
        });
    }
    public stringRemove(event: KeyboardEvent) {
        const question = `Do you really want to delete string ${this.jobSelection.entity.jobId}?`;
        this.confirmService.openYesNo('Delete String', question).subscribe(
            (result) => {
                if (result.isAction && (result.action.text === this.confirmService.ActionTextYes)) {
                    const id: number = parseInt(this.jobSelection.entity.jobId, 10);
                    this.hrmJobApiService.delete(id).subscribe((deleted) => {
                        this.jobSelection.broadcastClear();
                        this.stringListRefresh(event);
                    });
                }
            },
            (error) => {
                alert(`Internal error! ${JSON.stringify(error)}`);
            }
        );
    }
    public stringEdit(event: KeyboardEvent) {
        this.stringEditEntity(this.jobSelection.entity);
    }
    public stringEditEntity(entity: HrmJobListItem) {
        const id = entity.jobId;
        const existingTabIndex = this.tabstrip.getTabIndexById(id);
        if (existingTabIndex) {
            this.parentTabComponent.selectTab(existingTabIndex);
        } else {
            const title = `Edit String ${id}`;
            this.tabstrip.append({
                title,
                type: HrmJobDetailName,
                id,
                isNew: false,
            });
        }
    }
    public stringCopy(event: KeyboardEvent) {
        const id = this.jobSelection.entity.jobId;
        const title = `New String (copied from ${id})`;
        this.tabstrip.append({
            title,
            type: HrmJobDetailName,
            isNew: true,
            copyFromId: id
        });
    }
    public stringListRefresh(event: KeyboardEvent) {
        this.filters.next(this.filter);
    }

    public setProductionStatusList(statusList: ComboBoxItemNumberString[]) {
        this.productionStatusList = statusList;
    }

    // Input Slabs List toolbar button handlers
    public inputSlabView(event: KeyboardEvent) {
        this.inputSlabViewEntity(this.inputPieceSelection.entity);
    }
    public inputSlabViewEntity(entity: HrmInputPiece) {
        const id = entity.pieceNo;
        const existingTabIndex = this.tabstrip.getTabIndexById(id);
        if (existingTabIndex) {
            this.parentTabComponent.selectTab(existingTabIndex);
        } else {
            const title = `View Input Slab ${id}`;
            this.tabstrip.append({
                title,
                type: HrmInputPieceDetailName,
                id,
                isNew: false,
            });
        }
    }
    public inputSlabsListRefresh(event: KeyboardEvent) {
        this.jobSelection.broadcast();
    }

    public get newButtonVisible(): boolean {
        this.dumpSecurityRoleFlags();
        return this.isInAllRole;
    }

    public get removeButtonVisible(): boolean {
        return this.isInAllRole;
    }

    public get updateButtonVisible(): boolean {
        return this.isInAllRole || this.isInRestrictedRole;
    }

    public get copyButtonVisible(): boolean {
        return this.isInAllRole;
    }

}

