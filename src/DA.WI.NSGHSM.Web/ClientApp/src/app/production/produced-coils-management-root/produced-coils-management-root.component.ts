import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { Subject } from 'rxjs';

import {
    AuxValueService,
    BaseMasterDetailTabbedComponent,
    ComboBoxItemNumberString,
    ProducedCoilApiService,
    ProducedCoilSelectionHelper,
    UsedSetupSelectionHelper,
    ProducedCoilDetail,
    UsedSetupDetail,
    UsedSetupListItem,
    ProducedCoil,
} from '@app/shared';
import { ConfirmService } from '@app/widget';

import { ProducedCoilListFilterFromPcmRoot, ProducedCoilsManagementChildState } from './model';
import { AuthService } from '@app/core';

//const routerPath = ['production', 'produced-coils-management'];
//const routerPathProducedCoil = [...routerPath, 'produced-coil'];
//const routerPathUsedSetup = [...routerPath, 'used-setup'];

const ProducedCoilDetailName = 'ProducedCoilDetail';

@Component({
    selector: 'app-produced-coils-management-root',
    templateUrl: './produced-coils-management-root.component.html',
    styleUrls: ['./produced-coils-management-root.component.css']
})
export class ProducedCoilsManagementRootComponent
    extends BaseMasterDetailTabbedComponent
    implements OnInit, OnDestroy {

    // filters
    public filters: Subject<ProducedCoilListFilterFromPcmRoot>;
    public filter: ProducedCoilListFilterFromPcmRoot;

    public producedCoilStatusList: ComboBoxItemNumberString[] = [];
    public set producedCoilStatusValue(item: ComboBoxItemNumberString) {
        this.filter.coilStatus.value = item.value;
    }

    public producedCoilSelection: ProducedCoilSelectionHelper;
    public usedSetupSelection: UsedSetupSelectionHelper;

    constructor(
        private confirmService: ConfirmService,
        private auxValueService: AuxValueService,
        private producedCoilApiService: ProducedCoilApiService,
        authService: AuthService,
    ) {
        super(authService, 'Produced Coils');

        this.filters = new Subject<ProducedCoilListFilterFromPcmRoot>();
        this.filter = new ProducedCoilListFilterFromPcmRoot();

        this.producedCoilSelection = new ProducedCoilSelectionHelper();
        this.usedSetupSelection = new UsedSetupSelectionHelper();
        this.subscribe(
            this.auxValueService.producedCoilStateList.subscribe((list) => {
                this.producedCoilStatusList = list;
            }),
            this.tabstrip.tabClosed.subscribe((event) => {
                if (event.entityName === ProducedCoilDetailName){
                    this.filters.next(this.filter);
                } else {
                    // processed by superclass
                }
            })
        );
    }

    ngOnInit() {
        this.setupSecurityRoleFlags();
        this.filters.next(this.filter);
    }

    public applyFilters(event: KeyboardEvent) {
        this.filters.next(this.filter);
    }

    public producedCoilNew(event: KeyboardEvent) {
        const title = `New Produced Coil`;
        this.tabstrip.append({
            title,
            type: ProducedCoilDetailName,
            isNew: true,
        });
    }

    public producedCoilRemove(event: KeyboardEvent) {
        const question = `Do you really want to delete produced coil ${this.producedCoilSelection.entity.outPieceNo}?`;
        this.confirmService.openYesNo('Delete Produced Coil', question).subscribe(
            (result) => {
                if (result.isAction && (result.action.text === this.confirmService.ActionTextYes)) {
                    const id: number = this.producedCoilSelection.entity.outPieceNo;
                    this.producedCoilApiService.delete(id).subscribe((deleted) => {
                        this.producedCoilSelection.broadcastClear();
                    });
                }
            },
            (error) => {
                alert(`Internal error! ${JSON.stringify(error)}`);
            }
        );
    }

    public producedCoilEdit(event: KeyboardEvent) {
        this.producedCoilEditEntity(this.producedCoilSelection.entity);
    }
    public producedCoilEditEntity(entity: ProducedCoil) {
        const id = entity.outPieceNo;
        const existingTabIndex = this.tabstrip.getTabIndexById(id);
        if (existingTabIndex) {
            this.parentTabComponent.selectTab(existingTabIndex);
        } else {
            const title = `Edit Produced Coil ${entity.outPieceId}`;
            this.tabstrip.append({
                title,
                type: ProducedCoilDetailName,
                isNew: false,
                id,
            });
        }
    }

    public producedCoilCopy(event: KeyboardEvent) {
        const title = `New Produced Coil (copied from ${this.producedCoilSelection.entity.outPieceId})`;
        this.tabstrip.append({
            title,
            type: ProducedCoilDetailName,
            isNew: true,
            copyFromId: this.producedCoilSelection.entity.outPieceNo,
        });
    }

    public producedCoilListRefresh(event: KeyboardEvent) {
        this.filters.next(this.filter);
    }

    public usedSetupView(event: KeyboardEvent) {
        this.usedSetupViewEntity(this.usedSetupSelection.entity);
    }
    public usedSetupViewEntity(entity: UsedSetupListItem) {
        const id = entity.inPieceNo;
        const existingTabIndex = this.tabstrip.getTabIndexById(id);
        if (existingTabIndex) {
            this.parentTabComponent.selectTab(existingTabIndex);
        } else {
            const title = `View Used Setup ${id}`;
            this.tabstrip.append({
                title,
                type: UsedSetupDetail['name'],
                isNew: false,
                id,
            });
        }
    }

    public usedSetupSave(event: KeyboardEvent) {
        const question = `Do you really want to save this setup?`;
        this.confirmService.openYesNo('Save Setup', question).subscribe(
            (result) => {
                if (result.isAction && (result.action.text === this.confirmService.ActionTextYes)) {
                    // TODO
                    alert(`${ProducedCoilsManagementRootComponent['name']}.usedSetupSave()\n\nNOT IMPLEMENTED`);
                }
            },
            (error) => {
                alert(`Internal error! ${JSON.stringify(error)}`);
            }
        );
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
