import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { Subject } from 'rxjs';

import {
    AuxValueService,
    BaseMasterDetailTabbedComponent,
    ComboBoxItemNumberString,
    HrmInputPieceApiService,
    HrmInputPieceSelectionHelper,
    HrmInputPieceDetail,
    HrmInputPiece,
} from '@app/shared';
import { ConfirmService } from '@app/widget';

import { HrmInputPieceListFilterFromIslmRoot, } from './model';
import { StringsListManagementChildState } from '../strings-list-management-root';
import { AuthService } from '@app/core';


const HrmInputPieceDetailName = 'HrmInputPieceDetail';


@Component({
    selector: 'app-input-slabs-management-root',
    templateUrl: './input-slabs-management-root.component.html',
    styleUrls: ['./input-slabs-management-root.component.css']
})
export class InputSlabsManagementRootComponent
    extends BaseMasterDetailTabbedComponent
    implements OnInit, OnDestroy {

    private childState: StringsListManagementChildState = { pieceStatusList: [] };

    // filters
    public filters: Subject<HrmInputPieceListFilterFromIslmRoot>;
    public filter: HrmInputPieceListFilterFromIslmRoot;

    public get pieceStatusList(): ComboBoxItemNumberString[] {
        return this.childState.pieceStatusList;
    }
    public set filterPieceStatusValue(item: ComboBoxItemNumberString) {
        this.filter.pieceStatus.value = item.value;
    }

    // HrmInputPiece list
    public inputPieceSelection: HrmInputPieceSelectionHelper;

    constructor(
        private confirmService: ConfirmService,
        private auxValueService: AuxValueService,
        private hrmInputPieceApiService: HrmInputPieceApiService,
        authService: AuthService,
    ) {
        super(authService, 'Input Slabs');

        this.filters = new Subject<HrmInputPieceListFilterFromIslmRoot>();
        this.filter = new HrmInputPieceListFilterFromIslmRoot();

        this.inputPieceSelection = new HrmInputPieceSelectionHelper();
        this.subscribe(
            this.inputPieceSelection.subscribe(),
            this.auxValueService.statusInputPieceList.subscribe(
                (list) => {this.childState.pieceStatusList = list;
            }),
            this.tabstrip.tabClosed.subscribe((event) => {
                if (event.entityName === HrmInputPieceDetailName){
                    this.filters.next(this.filter);
                } else {
                    // processed by superclass
                }
            })
        );
    }

    ngOnInit() {
        this.setupSecurityRoleFlags();
    }

    public applyFilters(event: KeyboardEvent) {
        this.filters.next(this.filter);
    }

    // String List toolbar button handlers
    public inputSlabNew(event: KeyboardEvent) {
        const title = `New Input Slab`;
        this.tabstrip.append({
            title,
            type: HrmInputPieceDetailName,
            isNew: true,
        });
    }
    public inputSlabRemove(event: KeyboardEvent) {
        const question = `Do you really want to delete input slab ${this.inputPieceSelection.entity.pieceNo}?`;
        this.confirmService.openYesNo('Delete Input Slab', question).subscribe(
            (result) => {
                if (result.isAction && (result.action.text === this.confirmService.ActionTextYes)) {
                    const id: number = this.inputPieceSelection.entity.pieceNo;
                    this.hrmInputPieceApiService.delete(id).subscribe((deleted) => {
                        this.inputPieceSelection.broadcastClear();
                    });
                }
            },
            (error) => {
                alert(`Internal error! ${JSON.stringify(error)}`);
            }
        );
    }
    public inputSlabEdit(event: KeyboardEvent) {
        this.inputSlabEditEntity(this.inputPieceSelection.entity);
    }
    public inputSlabEditEntity(entity: HrmInputPiece) {
        /*
        const state: StringsListManagementChildState = {
            isNew: true,
            pieceStatusList: this.pieceStatusList,
        };
        const extras: NavigationExtras = { state, };
        this.router.navigate([...routerPathMaster, this.inputPieceSelection.entity.pieceNo], extras);
        */
        const id = entity.pieceNo;
        const existingTabIndex = this.tabstrip.getTabIndexById(id);
        if (existingTabIndex) {
            this.parentTabComponent.selectTab(existingTabIndex);
        } else {
            const title = `Edit Input Slab ${id}`;
            this.tabstrip.append({
                title,
                type: HrmInputPieceDetailName,
                isNew: false,
                id,
            });
        }
    }
    public inputSlabCopy(event: KeyboardEvent) {
        const title = `New Input Slab (copied from ${this.inputPieceSelection.entity.pieceNo})`;
        this.tabstrip.append({
            title,
            type: HrmInputPieceDetailName,
            isNew: true,
            copyFromId: this.inputPieceSelection.entity.pieceNo,
        });
    }
    public inputSlabListRefresh(event: KeyboardEvent) {
        this.filters.next(this.filter);
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
